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

        public InputWindow( String name)
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
                if (encryptRB.IsChecked == true)
                {

                } else
                {
                    // Set a variable to the My Documents path.
                    string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                    string personFilePrivate = mydocpath + @"\" + otherPersonTextBox.Text + "PrivateRSA.xml";
                    string personFilePublic = mydocpath + @"\" + otherPersonTextBox.Text + "PublicRSA.xml";
                    if (!File.Exists(personFilePrivate) || !File.Exists(personFilePublic))
                    {   
                        RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                        using (StreamWriter outputFile = new StreamWriter(personFilePrivate))
                        {
                            outputFile.WriteLine(RSA.ToXmlString(true));
                        }
                        using (StreamWriter outputFile = new StreamWriter(personFilePublic))
                        {
                            outputFile.WriteLine(RSA.ToXmlString(true));
                        }
                    }

                }
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
                fileLocation = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
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
