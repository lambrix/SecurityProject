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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SecurityAESProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 

        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != null && textBox.Text != "")
            {
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                // Set a variable to the My Documents path.
                string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                string personFilePrivate = mydocpath + @"\" + textBox.Text + "PrivateRSA.xml";
                string personFilePublic = mydocpath + @"\" + textBox.Text + "PublicRSA.xml";
                if (!File.Exists(personFilePrivate) || !File.Exists(personFilePublic))
                {
                    using (StreamWriter outputFile = new StreamWriter(personFilePrivate))
                    {
                        outputFile.WriteLine(RSA.ToXmlString(true));
                    }
                    using (StreamWriter outputFile = new StreamWriter(personFilePublic))
                    {
                        outputFile.WriteLine(RSA.ToXmlString(false));
                    }
                }
                InputWindow window = new InputWindow(textBox.Text);
                window.Show();
                this.Close();
            }
            
        }
    }
}
