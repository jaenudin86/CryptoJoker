namespace CryptoJokerDecryptor
{
    partial class KeyValidationWindow
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
            this.DecryptionKeyTxt = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SubmitDecryptionKeyBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DecryptionKeyTxt
            // 
            this.DecryptionKeyTxt.Location = new System.Drawing.Point(12, 29);
            this.DecryptionKeyTxt.Name = "DecryptionKeyTxt";
            this.DecryptionKeyTxt.Size = new System.Drawing.Size(391, 146);
            this.DecryptionKeyTxt.TabIndex = 0;
            this.DecryptionKeyTxt.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Put your decryption key here:";
            // 
            // SubmitDecryptionKeyBtn
            // 
            this.SubmitDecryptionKeyBtn.Location = new System.Drawing.Point(79, 194);
            this.SubmitDecryptionKeyBtn.Name = "SubmitDecryptionKeyBtn";
            this.SubmitDecryptionKeyBtn.Size = new System.Drawing.Size(262, 36);
            this.SubmitDecryptionKeyBtn.TabIndex = 2;
            this.SubmitDecryptionKeyBtn.Text = "Submit Decryption Key";
            this.SubmitDecryptionKeyBtn.UseVisualStyleBackColor = true;
            this.SubmitDecryptionKeyBtn.Click += new System.EventHandler(this.SubmitDecryptionKeyBtn_Click);
            // 
            // KeyValidationWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 250);
            this.Controls.Add(this.SubmitDecryptionKeyBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.DecryptionKeyTxt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyValidationWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Key Validator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox DecryptionKeyTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SubmitDecryptionKeyBtn;
    }
}