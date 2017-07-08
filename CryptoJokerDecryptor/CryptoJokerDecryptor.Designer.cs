namespace CryptoJokerDecryptor
{
    partial class CryptoJokerDecryptor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CryptoJokerDecryptor));
            this.CryptoMessageTxt = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PersonalIDTxt = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BitcoinAdressLbl = new System.Windows.Forms.Label();
            this.CopyPersonalIDBtn = new System.Windows.Forms.Button();
            this.CopyBitcoinAddressBtn = new System.Windows.Forms.Button();
            this.DecryptBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CryptoMessageTxt
            // 
            this.CryptoMessageTxt.Font = new System.Drawing.Font("Microsoft YaHei UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CryptoMessageTxt.Location = new System.Drawing.Point(12, 12);
            this.CryptoMessageTxt.Name = "CryptoMessageTxt";
            this.CryptoMessageTxt.Size = new System.Drawing.Size(637, 207);
            this.CryptoMessageTxt.TabIndex = 0;
            this.CryptoMessageTxt.Text = resources.GetString("CryptoMessageTxt.Text");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 262);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Personal ID:";
            // 
            // PersonalIDTxt
            // 
            this.PersonalIDTxt.AutoSize = true;
            this.PersonalIDTxt.Location = new System.Drawing.Point(112, 262);
            this.PersonalIDTxt.Name = "PersonalIDTxt";
            this.PersonalIDTxt.Size = new System.Drawing.Size(43, 13);
            this.PersonalIDTxt.TabIndex = 2;
            this.PersonalIDTxt.Text = "[HWID]";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 262);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Bitcoin Address:";
            // 
            // BitcoinAdressLbl
            // 
            this.BitcoinAdressLbl.AutoSize = true;
            this.BitcoinAdressLbl.Location = new System.Drawing.Point(432, 262);
            this.BitcoinAdressLbl.Name = "BitcoinAdressLbl";
            this.BitcoinAdressLbl.Size = new System.Drawing.Size(201, 13);
            this.BitcoinAdressLbl.TabIndex = 4;
            this.BitcoinAdressLbl.Text = "1yh3eJjuXwqqXgpu8stnojm148b8d6NFQ";
            // 
            // CopyPersonalIDBtn
            // 
            this.CopyPersonalIDBtn.Location = new System.Drawing.Point(12, 287);
            this.CopyPersonalIDBtn.Name = "CopyPersonalIDBtn";
            this.CopyPersonalIDBtn.Size = new System.Drawing.Size(296, 23);
            this.CopyPersonalIDBtn.TabIndex = 5;
            this.CopyPersonalIDBtn.Text = "Copy Personal ID";
            this.CopyPersonalIDBtn.UseVisualStyleBackColor = true;
            this.CopyPersonalIDBtn.Click += new System.EventHandler(this.CopyPersonalIDBtn_Click);
            // 
            // CopyBitcoinAddressBtn
            // 
            this.CopyBitcoinAddressBtn.Location = new System.Drawing.Point(337, 287);
            this.CopyBitcoinAddressBtn.Name = "CopyBitcoinAddressBtn";
            this.CopyBitcoinAddressBtn.Size = new System.Drawing.Size(296, 23);
            this.CopyBitcoinAddressBtn.TabIndex = 6;
            this.CopyBitcoinAddressBtn.Text = "Copy The Bitcoin Address";
            this.CopyBitcoinAddressBtn.UseVisualStyleBackColor = true;
            this.CopyBitcoinAddressBtn.Click += new System.EventHandler(this.CopyBitcoinAddressBtn_Click);
            // 
            // DecryptBtn
            // 
            this.DecryptBtn.Location = new System.Drawing.Point(12, 327);
            this.DecryptBtn.Name = "DecryptBtn";
            this.DecryptBtn.Size = new System.Drawing.Size(637, 37);
            this.DecryptBtn.TabIndex = 7;
            this.DecryptBtn.Text = "You got the decryption key ?? Cool, click me to decrypt your files !!";
            this.DecryptBtn.UseVisualStyleBackColor = true;
            this.DecryptBtn.Click += new System.EventHandler(this.DecryptBtn_Click);
            // 
            // CryptoJokerDecryptor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(661, 376);
            this.Controls.Add(this.DecryptBtn);
            this.Controls.Add(this.CopyBitcoinAddressBtn);
            this.Controls.Add(this.CopyPersonalIDBtn);
            this.Controls.Add(this.BitcoinAdressLbl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PersonalIDTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CryptoMessageTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "CryptoJokerDecryptor";
            this.ShowInTaskbar = false;
            this.Text = "Crypto Joker 1.0";
            this.Load += new System.EventHandler(this.CryptoJokerGUI_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CryptoMessageTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label PersonalIDTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label BitcoinAdressLbl;
        private System.Windows.Forms.Button CopyPersonalIDBtn;
        private System.Windows.Forms.Button CopyBitcoinAddressBtn;
        private System.Windows.Forms.Button DecryptBtn;
    }
}

