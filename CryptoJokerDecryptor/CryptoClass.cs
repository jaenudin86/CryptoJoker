using System;
using System.IO;
using System.Security.Cryptography;

namespace CryptoJokerDecryptor
{//http://www.csharpdeveloping.net/Snippet/how_to_encrypt_decrypt_using_asymmetric_algorithm_rsa
    internal class CryptoClass
    {
        private readonly Random _rnd = new Random();
        private readonly bool _optimalAsymmetricEncryptionPadding = false;
        public string EncryptionKey { get; private set; }

        public CryptoClass()
        {
            EncryptionKey = GenerateRandomString(20);
        }

        public CryptoClass(string encryptionKey)
        {
            EncryptionKey = encryptionKey;
        }

        public void GenerateKeys(int keySize, out string publicKey, out string publicAndPrivateKey)
        {
            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                publicKey = provider.ToXmlString(false);
                publicAndPrivateKey = provider.ToXmlString(true);
            }
        }

        public byte[] RsaEncryption(byte[] data, int keySize, string publicKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            int maxLength = GetMaxDataLength(keySize);
            if (data.Length > maxLength) throw new ArgumentException($"Maximum data length is {maxLength}", "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicKeyXml)) throw new ArgumentException("Key is null or empty", "publicKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicKeyXml);
                return provider.Encrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        public byte[] RsaDecryption(byte[] data, int keySize, string publicAndPrivateKeyXml)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Data are empty", "data");
            if (!IsKeySizeValid(keySize)) throw new ArgumentException("Key size is not valid", "keySize");
            if (String.IsNullOrEmpty(publicAndPrivateKeyXml)) throw new ArgumentException("Key is null or empty", "publicAndPrivateKeyXml");

            using (var provider = new RSACryptoServiceProvider(keySize))
            {
                provider.FromXmlString(publicAndPrivateKeyXml);
                return provider.Decrypt(data, _optimalAsymmetricEncryptionPadding);
            }
        }

        public void EncryptFileFully(string filePath)
        {
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            byte[] bytesToEncrypt = new byte[fileStream.Length];
            using (BinaryReader brReader = new BinaryReader(fileStream))
            {
                using (BinaryWriter brWriter = new BinaryWriter(fileStream))
                {
                    brReader.BaseStream.Position = 0;
                    bytesToEncrypt = brReader.ReadBytes((int)fileStream.Length);
                    brWriter.BaseStream.Position = 0;
                    bytesToEncrypt = EncodeAob(bytesToEncrypt, GetBytes(EncryptionKey));
                    brWriter.Write(bytesToEncrypt);
                }
            }
            fileStream.Close();
            fileStream.Dispose();
        }

        public void DecryptFileFully(string filePath)
        {
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            byte[] bytesToDecrypt = new byte[fileStream.Length];
            using (BinaryReader brReader = new BinaryReader(fileStream))
            {
                using (BinaryWriter brWriter = new BinaryWriter(fileStream))
                {
                    brReader.BaseStream.Position = 0;
                    bytesToDecrypt = brReader.ReadBytes((int)fileStream.Length);
                    brWriter.BaseStream.Position = 0;
                    bytesToDecrypt = DecodeAob(bytesToDecrypt, GetBytes(EncryptionKey));
                    brWriter.Write(bytesToDecrypt);
                }
            }
            fileStream.Close();
            fileStream.Dispose();
        }

        public void EncryptFilePartially(string filePath)
        {
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            byte[] bytesToEncrypt = new byte[1024];
            using (BinaryReader brReader = new BinaryReader(fileStream))
            {
                using (BinaryWriter brWriter = new BinaryWriter(fileStream))
                {
                    //go to the the beginning of the file and read the first 1024 bytes
                    brReader.BaseStream.Position = 0;
                    bytesToEncrypt = brReader.ReadBytes(1024);
                    //go to the beginning of the file, encrypt and write changes to file
                    brWriter.BaseStream.Position = 0;
                    bytesToEncrypt = EncodeAob(bytesToEncrypt, GetBytes(EncryptionKey));
                    brWriter.Write(bytesToEncrypt);
                }
            }
            fileStream.Close();
            fileStream.Dispose();
        }

        public void DecryptFilePartially(string filePath)
        {
            Stream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            byte[] bytesToDecrypt = new byte[1024];
            using (BinaryReader brReader = new BinaryReader(fileStream))
            {
                using (BinaryWriter brWriter = new BinaryWriter(fileStream))
                {
                    brReader.BaseStream.Position = 0;
                    bytesToDecrypt = brReader.ReadBytes(1024);
                    brWriter.BaseStream.Position = 0;
                    bytesToDecrypt = DecodeAob(bytesToDecrypt, GetBytes(EncryptionKey));
                    brWriter.Write(bytesToDecrypt);
                }
            }
            fileStream.Close();
            fileStream.Dispose();
        }

        private byte[] EncodeAob(byte[] aobToEncode, byte[] passwordBytes)
        {
            byte[] buildedArray = new byte[aobToEncode.Length];
            int currentByteCounter = 0;
            for (int i = 0; i < aobToEncode.Length; i++)
            {
                buildedArray[i] = (byte)(aobToEncode[i] + passwordBytes[currentByteCounter]);
                if (passwordBytes[currentByteCounter + 1] == 0x00)
                    currentByteCounter = 0;
                else
                    currentByteCounter++;
            }
            return buildedArray;
        }

        private byte[] DecodeAob(byte[] aobToDecode, byte[] passwordBytes)
        {
            byte[] buildedArray = new byte[aobToDecode.Length];
            int currentByteCounter = 0;
            for (int i = 0; i < aobToDecode.Length; i++)
            {
                buildedArray[i] = (byte)(aobToDecode[i] - passwordBytes[currentByteCounter]);
                if (passwordBytes[currentByteCounter + 1] == 0x00)
                    currentByteCounter = 0;
                else
                    currentByteCounter++;
            }
            return buildedArray;
        }

        private string GenerateRandomString(int length)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string buildedString = "";

            for (int i = 0; i < length; i++)
            {
                buildedString += chars[_rnd.Next(chars.Length)];
            }

            return buildedString;
        }

        private int GetMaxDataLength(int keySize)
        {
            if (_optimalAsymmetricEncryptionPadding)
            {
                return ((keySize - 384) / 8) + 7;
            }
            return ((keySize - 384) / 8) + 37;
        }

        private bool IsKeySizeValid(int keySize)
        {
            return keySize >= 384 &&
                   keySize <= 16384 &&
                   keySize % 8 == 0;
        }

        private byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        private string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }
    }
}
