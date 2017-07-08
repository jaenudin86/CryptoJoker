using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CryptoJokerDecryptor
{
    public partial class DecryptionProgressBarForm : Form
    {
        public DecryptionProgressBarForm()
        {
            InitializeComponent();
        }

        private void DecryptionProgressBarForm_Load(object sender, EventArgs e)
        {
            DecryptionProcessProgressBar.Style = ProgressBarStyle.Marquee;
            DecryptionProcessProgressBar.MarqueeAnimationSpeed = 30;
        }
    }
}
