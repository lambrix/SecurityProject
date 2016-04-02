using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SecurityAESProject
{
    /// <summary>
    /// Interaction logic for InputWindow.xaml
    /// </summary>

    public partial class InputWindow : Window
    {
        private String userName;
        private String welcome = "Welcome ";
        private String fileLocation;
        private string personFilePublic;
        string pathfolder;

        public InputWindow(String name)
        {
            InitializeComponent();
            this.userName = name;
            welcome = welcome + userName;
            nameLabel.Content = welcome;

        }

        private void nextButtonClick(object sender, RoutedEventArgs e)
        {
            if (otherPersonTextBox.Text != "" && otherPersonTextBox.Text != null && fileLocation != "" && fileLocation != null)
            {
                // Set a variable to the My Documents path.
                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string personFilePrivate = mydocpath + @"\" + otherPersonTextBox.Text + "PrivateRSA.xml";
                personFilePublic = mydocpath + @"\" + otherPersonTextBox.Text + "PublicRSA.xml";
                if (!File.Exists(personFilePrivate) || !File.Exists(personFilePublic))
                {
                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    using (StreamWriter outputFile = new StreamWriter(personFilePrivate))
                    {
                        outputFile.WriteLine(RSA.ToXmlString(true));
                    }
                    using (StreamWriter outputFile = new StreamWriter(personFilePublic))
                    {
                        outputFile.WriteLine(RSA.ToXmlString(false));
                    }
                }

                if (encryptRB.IsChecked == true)
                {

                    Byte[] input = File.ReadAllBytes(fileLocation);
                    Byte[] output = AESEncrypt(input);
                    
                    //wegschrijven
                    string newPath = pathfolder + "\\beveiligd-bestand.txt";
                    File.WriteAllBytes(newPath, output);
                    MessageBox.Show("2/3 done\ntodo: hash");

                    //hash maken bestand
                    MessageBox.Show("encryptie gelukt");
                }
                else
                {


                }
            }
        }

        //byte array encrypteren via aes methode
        private byte[] AESEncrypt(byte[] input)
        {
            byte[] encryptedBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (AesManaged AES = new AesManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    AES.GenerateKey(); //sleutel generen
                    //eventueel IV nog

                    //sleutel bijhouden, later encrypteren met RSA via publieke sleutel van de andere
                    byte[] key = AES.Key;
                
                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();

                    //AESsleutel encrypteren
                    KeyEncrypter(key);
                }
            }
            return encryptedBytes;
        }

        private void KeyEncrypter(byte[] key)
        {
         
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                string publickKey = null;

                // publieke key importeren:
                using (StreamReader inputFile = new StreamReader(personFilePublic))
                {
                    publickKey = inputFile.ReadLine();
                }
                rsa.FromXmlString(publickKey);

                //encrypteren
                byte[] encrypted = rsa.Encrypt(key, true);
                
                //folder aanmaken voor bestanden op te slaan
                int l = fileLocation.LastIndexOf('.');
                pathfolder = fileLocation.Substring(0, l);
                Directory.CreateDirectory(pathfolder);
                
                //gegevens in een bestand wegschrijven
                string path = pathfolder + "\\sleutel.txt";
                File.WriteAllBytes(path, encrypted);
                MessageBox.Show("1/3 Done");

            }
        }
        private void opendialogButtonClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "All Files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            if (openFileDialog.ShowDialog() == true)
            {
                //fileLocation = System.IO.Path.GetDirectoryName(openFileDialog.FileName); We hebben de file nodig niet het pad
                fileLocation = System.IO.Path.GetFullPath(openFileDialog.FileName);
                pathLabel.Content = fileLocation;

            }
        }

        private void DropList_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent("myFormat") ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private void DropList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                fileLocation = files[0];
                pathLabel.Content = fileLocation;
            }
        }

        private void hyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void decrypt()
        {
            RijndaelManaged AES = new RijndaelManaged();
            AES.KeySize = 256;
            AES.BlockSize = 128;

        }
    }
}
