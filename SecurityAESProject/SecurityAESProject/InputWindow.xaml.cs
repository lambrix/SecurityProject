using Microsoft.Win32;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

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
        private String extension;
        private string personFilePublic;
        string pathfolder;
        //stega. var
        private Bitmap globalBitmap;
        private BitmapImage bitmapImage;
        private string extractedText = string.Empty;
        private string pathfile = string.Empty;

        public InputWindow(String name, String extension)
        {
            InitializeComponent();
            this.userName = name;
            this.extension = extension;
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

                    MainWindow window = new MainWindow(extension);
                    window.Show();
                    this.Close();
                }
                else
                {
                    Byte[] input = File.ReadAllBytes(fileLocation);
                    AESDecrypt(input);
                }
            }else if(!(otherPersonTextBox.Text != "" && otherPersonTextBox.Text != null))
            {
                MessageBox.Show("Geef een naam op");
            }
            else if (!(fileLocation != "" && fileLocation != null))
            {
                MessageBox.Show("Geef een bestand op");
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
                    int pos = l + 1;
                    extension = fileLocation.Substring(pos, fileLocation.Length - pos);
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
            //byte[] HashValue = SHhash.ComputeHash(input);
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
                byte[] encrypted = rsa.SignData(input, SHhash);// test false 

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

        private void opendialogButtonClickImage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogImage = new OpenFileDialog();
            openFileDialogImage.Filter = "Image Files (*.png*)|*.png*";
            openFileDialogImage.FilterIndex = 1;
            openFileDialogImage.Multiselect = false;

            if (openFileDialogImage.ShowDialog() == true)
            {
                //fileLocation = System.IO.Path.GetDirectoryName(openFileDialog.FileName); We hebben de file nodig niet het pad
                fileLocation = System.IO.Path.GetFullPath(openFileDialogImage.FileName);
                imagePathLabel.Content = fileLocation;
                imagePictureBox.Source = new BitmapImage(new Uri(openFileDialogImage.FileName));
                

            }
        }

        private void opendialogButtonClickText(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogText = new OpenFileDialog();
            openFileDialogText.Filter = "Zip Files|*.zip;*.rar";
            openFileDialogText.Filter = "All files|*.*";
            openFileDialogText.FilterIndex = 1;
            openFileDialogText.Multiselect = false;

            if (openFileDialogText.ShowDialog() == true)
            {
                //fileLocation = System.IO.Path.GetDirectoryName(openFileDialog.FileName); We hebben de file nodig niet het pad
                fileLocation = System.IO.Path.GetFullPath(openFileDialogText.FileName);
                textPathLabel.Content = fileLocation;
                textToBeHidden.Text = File.ReadAllText(openFileDialogText.FileName);
                textToBeHidden.TextWrapping = TextWrapping.Wrap;
            }
        }

        private void hideTextInImage(Object sender, RoutedEventArgs e)
        {
            bitmapImage = (BitmapImage)imagePictureBox.Source;
            
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                globalBitmap = new Bitmap(outStream);
            }
            
            //zip to string
            string pathfile = textPathLabel.Content.ToString();
            byte[] test = File.ReadAllBytes(pathfile);
            
            //MessageBox.Show(string.Join("-", test));
            //string zipfileString2 = BitConverter.ToString(test);
            //string zipfileString = Encoding.ASCII.GetString(test);
            string zipfileString = string.Join("-", test);
            //Encoding.Convert(Encoding.ASCII, Encoding.UTF8, test);
            //MessageBox.Show(zipfileString);
            if (test.Equals("") || test.Length == 0)
            {
                MessageBox.Show("The text you want to hide can't be empty", "Warning");

                return;
            }

            globalBitmap = SteganographyHelper.embedText(zipfileString, globalBitmap);
            MessageBox.Show("Your text was hidden in the image successfully!", "Done");

            using (var memory = new MemoryStream())
            {
                globalBitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var localBitmapImage = new BitmapImage();
                localBitmapImage.BeginInit();
                localBitmapImage.StreamSource = memory;
                localBitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                localBitmapImage.EndInit();

                bitmapImage = localBitmapImage;
            }

            imagePictureBox.Source = bitmapImage;
            //notesLabel.Text = "Notes: don't forget to save your new image.";
            //notesLabel.ForeColor = Color.Red;
        }

        private void extractTextFromImage(Object sender, RoutedEventArgs e)
        {
            string extractedText = SteganographyHelper.extractText(globalBitmap);
            //extractedText = SteganographyHelper.extractText(bmp);
            //dataTextBox.Text = extractedText;
            // string terug naar file en saven
            // eerst splitten en dan terug zetten naar een byte array
            //MessageBox.Show(extractedText);
            string[] arr = extractedText.Split('-');
            int[] intArr = Array.ConvertAll(arr, element => int.Parse(element));


            byte[] array = new byte[arr.Length];
            char[] chars = intArr.Select(x => (char)x).ToArray();
            string str = new string(chars);
            MessageBox.Show(str);
            byte[] byteArray = Encoding.Default.GetBytes(chars);

            //for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i], 16);
            //byte[] toBytes = Encoding.Default.GetBytes(extractedText);
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Zip Files|*.zip;*.rar|Text files (*.txt)|*.txt";
            //save_dialog.Filter = "All files|*.*";
            if (save_dialog.ShowDialog() == true)
            {
                File.WriteAllBytes(save_dialog.FileName, byteArray);
                //File.WriteAllBytes(save_dialog.FileName, toBytes);
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

                    //
                    byte[] decryptedKey = KeyDecrypten();
                    if (decryptedKey != null && decryptedKey.Length > 0)
                    {
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
                        string path = rightLocation + "\\plaintext."+extension;
                        File.WriteAllText(path, plaintext);
                        MessageBox.Show("Bestand gedecrypteerd");

                        string hashpath = rightLocation + "\\hash.txt";
                        byte[] hashencrypted = File.ReadAllBytes(hashpath);
                        HashDecrypter(hashencrypted);

                        //HashDecrypter();
                    }
                    else
                    {
                        MainWindow window = new MainWindow();
                        window.Show();
                        this.Close();
                    }





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

                try
                {
                    byte[] decrypted = rsa.Decrypt(RSAKey, true);

                    //gegevens in een bestand wegschrijven
                    string path = rightLocation + "\\dsleutel.txt";
                    File.WriteAllBytes(path, decrypted);

                    return decrypted;
                }
                catch (CryptographicException ex)
                {
                    MessageBox.Show("Sorry, the file is not meant for you!");
                    return null;
                }

            }
        }

        private void HashDecrypter(byte[] input)
        {
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //publieke sleutel op halen
                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string personFilePublic = mydocpath + @"\" + otherPersonTextBox.Text + "PublicRSA.xml";
                //MessageBox.Show(personFilePublic);

                string publicKey;
                using (StreamReader inputFile = new StreamReader(personFilePublic))
                {
                    publicKey = inputFile.ReadLine();
                }

                rsa.FromXmlString(publicKey);
                //MessageBox.Show(publicKey);
                pathfolder = pathfolder.Substring(0, pathfolder.LastIndexOf('\\'));
                string plainTextFile = pathfolder + @"\plaintext."+extension;
                byte[] plainText = File.ReadAllBytes(plainTextFile);

                SHA256Managed SHhash = new SHA256Managed();
                byte[] hashedPlainText = SHhash.ComputeHash(plainText);
                bool correct = rsa.VerifyData(plainText, SHhash, input);
                //MessageBox.Show(correct+"");

                    
                //string path = pathfolder + "\\hashdecrypted.txt";
                //File.WriteAllBytes(path, decrypted);

                if (correct)
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
