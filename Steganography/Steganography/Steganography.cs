using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace Steganography
{
    public partial class Steganography : Form
    {
        private Bitmap bmp = null;
        private string extractedText = string.Empty;
        private string pathfile = string.Empty;

        public Steganography()
        {
            InitializeComponent();
        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;
            //zip to string
            string text = dataTextBox.Text;
            byte[] test = File.ReadAllBytes(pathfile);
            string zipfileString = BitConverter.ToString(test);
            if (text.Equals("") || test.Length == 0)
            {
                MessageBox.Show("The text you want to hide can't be empty", "Warning");

                return;
            }

            //if (encryptCheckBox.Checked)
            //{
            //    if (passwordTextBox.Text.Length < 6)
            //    {
            //        MessageBox.Show("Please enter a password with at least 6 characters", "Warning");

            //        return;
            //    }
            //    else
            //    {
            //        text = Crypto.EncryptStringAES(text, passwordTextBox.Text);
            //    }
            //}

            bmp = SteganographyHelper.embedText(zipfileString, bmp);
            MessageBox.Show("Your text was hidden in the image successfully!", "Done");
            imagePictureBox.Image = bmp;
            notesLabel.Text = "Notes: don't forget to save your new image.";
            notesLabel.ForeColor = Color.Red;
        }

        private void extractButton_Click(object sender, EventArgs e)
        {
            bmp = (Bitmap)imagePictureBox.Image;
            string extractedText = SteganographyHelper.extractText(bmp);
            //extractedText = SteganographyHelper.extractText(bmp);
            //dataTextBox.Text = extractedText;
            // string terug naar file en saven
            // eerst splitten en dan terug zetten naar een byte array
            String[] arr = extractedText.Split('-');
            byte[] array = new byte[arr.Length];
            for (int i = 0; i < arr.Length; i++) array[i] = Convert.ToByte(arr[i],16);

            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Zip Files|*.zip;*.rar";
            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(save_dialog.FileName, array);
            }

        }

        private void imageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files (*.jpeg; *.png; *.bmp)|*.jpg; *.png; *.bmp";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                imagePictureBox.Image = Image.FromFile(open_dialog.FileName);
            }
        }

        private void imageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Png Image|*.png|Bitmap Image|*.bmp";

            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                switch (save_dialog.FilterIndex)
                {
                    case 0:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Png);
                        }break;
                    case 1:
                        {
                            bmp.Save(save_dialog.FileName, ImageFormat.Bmp);
                        } break;
                }

                notesLabel.Text = "Notes:";
                notesLabel.ForeColor = Color.Black;
            }
        }

        private void textToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save_dialog = new SaveFileDialog();
            save_dialog.Filter = "Text Files|*.txt";

            if (save_dialog.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(save_dialog.FileName, dataTextBox.Text);
            }
        }

        private void textToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Text Files|*.txt";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                dataTextBox.Text = File.ReadAllText(open_dialog.FileName);
            }
        }


        private void select_file(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Zip Files|*.zip;*.rar";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                dataTextBox.Text = open_dialog.FileName;
                pathfile = open_dialog.FileName;
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}