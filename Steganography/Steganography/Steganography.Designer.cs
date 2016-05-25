namespace Steganography
{
    partial class Steganography
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Steganography));
            this.hideButton = new System.Windows.Forms.Button();
            this.dataTextBox = new System.Windows.Forms.TextBox();
            this.extractButton = new System.Windows.Forms.Button();
            this.imagePictureBox = new System.Windows.Forms.PictureBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notesLabel = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // hideButton
            // 
            this.hideButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hideButton.Location = new System.Drawing.Point(16, 657);
            this.hideButton.Margin = new System.Windows.Forms.Padding(4);
            this.hideButton.Name = "hideButton";
            this.hideButton.Size = new System.Drawing.Size(241, 57);
            this.hideButton.TabIndex = 0;
            this.hideButton.Text = "Hide";
            this.hideButton.UseVisualStyleBackColor = true;
            this.hideButton.Click += new System.EventHandler(this.hideButton_Click);
            // 
            // dataTextBox
            // 
            this.dataTextBox.Font = new System.Drawing.Font("Tahoma", 12F);
            this.dataTextBox.Location = new System.Drawing.Point(16, 386);
            this.dataTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.dataTextBox.Multiline = true;
            this.dataTextBox.Name = "dataTextBox";
            this.dataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dataTextBox.Size = new System.Drawing.Size(497, 139);
            this.dataTextBox.TabIndex = 2;
            // 
            // extractButton
            // 
            this.extractButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extractButton.Location = new System.Drawing.Point(273, 657);
            this.extractButton.Margin = new System.Windows.Forms.Padding(4);
            this.extractButton.Name = "extractButton";
            this.extractButton.Size = new System.Drawing.Size(241, 57);
            this.extractButton.TabIndex = 3;
            this.extractButton.Text = "Extract";
            this.extractButton.UseVisualStyleBackColor = true;
            this.extractButton.Click += new System.EventHandler(this.extractButton_Click);
            // 
            // imagePictureBox
            // 
            this.imagePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("imagePictureBox.Image")));
            this.imagePictureBox.Location = new System.Drawing.Point(16, 33);
            this.imagePictureBox.Margin = new System.Windows.Forms.Padding(4);
            this.imagePictureBox.Name = "imagePictureBox";
            this.imagePictureBox.Size = new System.Drawing.Size(499, 346);
            this.imagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imagePictureBox.TabIndex = 4;
            this.imagePictureBox.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(527, 28);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem1});
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // imageToolStripMenuItem1
            // 
            this.imageToolStripMenuItem1.Name = "imageToolStripMenuItem1";
            this.imageToolStripMenuItem1.Size = new System.Drawing.Size(181, 26);
            this.imageToolStripMenuItem1.Text = "Image";
            this.imageToolStripMenuItem1.Click += new System.EventHandler(this.imageToolStripMenuItem1_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.imageToolStripMenuItem});
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(181, 26);
            this.imageToolStripMenuItem.Text = "Image";
            this.imageToolStripMenuItem.Click += new System.EventHandler(this.imageToolStripMenuItem_Click);
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.notesLabel.Location = new System.Drawing.Point(20, 730);
            this.notesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(69, 25);
            this.notesLabel.TabIndex = 9;
            this.notesLabel.Text = "Notes:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(25, 532);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(232, 35);
            this.button1.TabIndex = 10;
            this.button1.Text = "Select file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.select_file);
            // 
            // Steganography
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(527, 766);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.imagePictureBox);
            this.Controls.Add(this.extractButton);
            this.Controls.Add(this.dataTextBox);
            this.Controls.Add(this.hideButton);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Steganography";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Steganography";
            ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button hideButton;
        private System.Windows.Forms.TextBox dataTextBox;
        private System.Windows.Forms.Button extractButton;
        private System.Windows.Forms.PictureBox imagePictureBox;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.Button button1;
    }
}

