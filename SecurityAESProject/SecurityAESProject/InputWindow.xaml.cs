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
                pathfolder = mydocpath;
                //files van andere persoon
                string personFilePrivate = mydocpath + @"\" + otherPersonTextBox.Text + "PrivateRSA.xml";
                personFilePublic = mydocpath + @"\" + otherPersonTextBox.Text + "PublicRSA.xml";
                if (!File.Exists(personFilePrivate) || !File.Exists(personFilePublic))
                {
                    RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                    using (StreamWriter outputFile = new StreamWriter(personFilePublic))
                    {
                        outputFile.WriteLine(RSA.ToXmlString(false));
                    }
                    using (StreamWriter outputFile = new StreamWriter(personFilePrivate))
                    {
                        outputFile.WriteLine(RSA.ToXmlString(true));
                    }

                }

                if (encryptRB.IsChecked == true)
                {

                    Byte[] input = File.ReadAllBytes(fileLocation);
                    Byte[] output = AESEncrypt(input);

                    //wegschrijven
                    string newPath = pathfolder + "\\beveiligd-bestand.txt";
                    File.WriteAllBytes(newPath, output);

                    MessageBox.Show("encryptie gelukt");
                }
                else
                {
                    Byte[] input = File.ReadAllBytes(fileLocation);
                    AESDecrypt(input);



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
                    AES.GenerateIV();
                    //eventueel IV nog

                    //sleutel bijhouden, later encrypteren met RSA via publieke sleutel van de andere
                    byte[] key = AES.Key;

                    byte[] IV = AES.IV;
                    //folder aanmaken voor bestanden op te slaan
                    int l = fileLocation.LastIndexOf('.');
                    pathfolder = fileLocation.Substring(0, l);
                    Directory.CreateDirectory(pathfolder);

                    //gegevens in een bestand wegschrijven
                    string path = pathfolder + "\\oldsleutel.txt";
                    File.WriteAllBytes(path, key);

                    string IVpath = pathfolder + "\\IV.txt";
                    File.WriteAllBytes(IVpath, IV);

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(input, 0, input.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();

                    //AESsleutel encrypteren
                    KeyEncrypter(key);
                    //hash maken van orgineel bestand
                    HashEncrypter(input);
                }
            }
            return encryptedBytes;
        }

        private void HashEncrypter(byte[] input)
        {
            //hash maken van orgineel bestand en encrypteren met privesleutel

            SHA256Managed SHhash = new SHA256Managed();
            byte[] HashValue = SHhash.ComputeHash(input);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string personFilePrivate = mydocpath + @"\" + userName + "PrivateRSA.xml";
                string privateKey;
                using (StreamReader inputFile = new StreamReader(personFilePrivate))
                {
                    privateKey = inputFile.ReadLine();
                }
                rsa.FromXmlString(privateKey);

                //encrypteren
                byte[] encrypted = rsa.Encrypt(HashValue, false);// test false 

                //gegevens in een bestand wegschrijven
                string path = pathfolder + "\\hash.txt";
                File.WriteAllBytes(path, encrypted);
                MessageBox.Show("2/3 Done");
            }
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

        private void AESDecrypt(byte[] input)
        {
            string plaintext = null;
            //byte[] decryptedBytes = null;
            using (MemoryStream ms = new MemoryStream(input))
            {
                using (AesManaged AES = new AesManaged())
                {
                    //instanties van AES aanmaken.

                    int l = fileLocation.LastIndexOf('.');
                    pathfolder = fileLocation.Substring(0, l);
                    string rightLocation = pathfolder.Substring(0, pathfolder.LastIndexOf('\\'));
                    string keyPath = rightLocation + "\\IV.txt";
                    Byte[] IVFile = File.ReadAllBytes(keyPath);

                    //decrypten lukt nu.
                    byte[] decryptedKey = KeyDecrypten();

                    AES.Key = decryptedKey;
                    AES.IV = IVFile;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (var srDcrypt = new StreamReader(cs))
                        {
                            plaintext = srDcrypt.ReadToEnd();
                        }
                    }

                    //gegevens in een bestand wegschrijven
                    string path = rightLocation + "\\plaintext.txt";
                    File.WriteAllText(path, plaintext);

                    //HashDecrypter(input);

                    string hashpath = rightLocation + "\\hash.txt";
                    byte[] hashencrypted = File.ReadAllBytes(hashpath);
                    HashDecrypter(hashencrypted);
                }
            }
        }

        private byte[] KeyDecrypten()
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                int l = fileLocation.LastIndexOf('.');
                pathfolder = fileLocation.Substring(0, l);
                string rightLocation = pathfolder.Substring(0, pathfolder.LastIndexOf('\\'));
                Directory.CreateDirectory(rightLocation);

                string privateKey = null;
                string keyPath = rightLocation + "\\sleutel.txt";
                Byte[] RSAKey = File.ReadAllBytes(keyPath);

                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string myPrivateKey = mydocpath + @"\" + userName + "PrivateRSA.xml";

                using (StreamReader inputFile = new StreamReader(myPrivateKey))
                {
                    privateKey = inputFile.ReadLine();
                }

                rsa.FromXmlString(privateKey);
                byte[] decrypted = rsa.Decrypt(RSAKey, true);

                //gegevens in een bestand wegschrijven
                string path = rightLocation + "\\dsleutel.txt";
                File.WriteAllBytes(path, decrypted);

                return decrypted;
            }
        }

        private void HashDecrypter(byte[] input)
        {
            //hash maken van orgineel bestand en encrypteren met privesleutel

            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //publieke sleutel op halen
                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string personFilePublic = mydocpath + @"\" + otherPersonTextBox.Text + "PublicRSA.xml";
                MessageBox.Show(personFilePublic);
                string publicKey;
                using (StreamReader inputFile = new StreamReader(personFilePublic))
                {
                    publicKey = inputFile.ReadLine();
                }

                rsa.FromXmlString(publicKey);
                MessageBox.Show(publicKey);
                //decrypteren
                byte[] decrypted = rsa.Decrypt(input, true);
                //
                //gegevens in een bestand wegschrijven
                string path = pathfolder + "\\hashdecrypted.txt";
                //File.WriteAllBytes(path, decrypted);
                MessageBox.Show("2/3 Done");

                //hash generen om te checken als die hetzelfde is 
                SHA256Managed SHhash = new SHA256Managed();
                pathfolder = pathfolder.Substring(0, pathfolder.LastIndexOf('\\'));
                byte[] HashValue = SHhash.ComputeHash(File.ReadAllBytes(pathfolder + "\\plaintext.txt"));

                MessageBox.Show(HashValue.ToString() +"\n\n" + decrypted.ToString());
                bool areEqual = HashValue.SequenceEqual(decrypted);
                if (areEqual)
                {
                    MessageBox.Show("hashes zijn gelijk");
                }
                else
                {
                    MessageBox.Show("hashes zijn verschillend");
                }

            }
        }
    }
}
