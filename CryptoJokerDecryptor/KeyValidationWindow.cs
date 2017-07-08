using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Xsl;

namespace CryptoJokerDecryptor
{
    public partial class KeyValidationWindow : Form
    {
        const int PublicPrivateKeyLength = 2440;
        public string PublicPrivateKey = string.Empty;

        public KeyValidationWindow()
        {
            InitializeComponent();
        }

        private void SubmitDecryptionKeyBtn_Click(object sender, EventArgs e)
        {
            if (DecryptionKeyTxt.Text.Length == PublicPrivateKeyLength)
            {
                string decodedPublicPrivateKey = DecodeString(DecryptionKeyTxt.Text);
                if (decodedPublicPrivateKey != null && decodedPublicPrivateKey.Contains("<RSAKeyValue>"))
                {
                    PublicPrivateKey = decodedPublicPrivateKey;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The decryption key has bad format.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The decryption key has bad format.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private string DecodeString(string str)
        {
            try
            {
                return GetString(Convert.FromBase64String(str));
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
