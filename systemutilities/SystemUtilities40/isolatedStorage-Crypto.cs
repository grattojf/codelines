// sysutils : SysUtils
// Écrit par : J.F.Gratton, 2013.01.03 23:13
// isolatedStorage-crypto.cs : crypto API for Isolated Storage files

// API written by Michael Björn, http://fiercedesign.wordpress.com/2011/11/03/windows-phone-7-encrypt-data/


using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Security.Cryptography;
using System.Text;

// ReSharper disable CheckNamespace
namespace JFG.SysUtils.IsolatedStorageCrypto
// ReSharper restore CheckNamespace
{
    /*
    public static class CryptoUtil
    {
        /// <summary>
        /// Encrypt a string and store it in the phone's isolated storage
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path"></param>
        public static void EncryptAndStore(string value, string path)
        {
            // Convert the string to a byte[].
            byte[] pinByte = Encoding.UTF8.GetBytes(value);

            // Encrypt the string by using the Protect() method.
            byte[] protectedBytes = ProtectedData.Protect(pinByte, null, DataProtectionScope.CurrentUser);

            // Store the encrypted string in isolated storage.
            WriteProtectedStringToFile(protectedBytes, path);
        }

        /// <summary>
        /// Encrypt a string and returns it into a string representation
        /// </summary>
        /// <param name="value"></param>
        /// <returns>the encoded string</returns>
        public static string EncryptAndReturn(string value)
        {
            // Convert the string to a byte[].
            byte[] pinByte = Encoding.UTF8.GetBytes(value);

            // Encrypt the string by using the Protect() method.
            byte[] protectedBytes = ProtectedData.Protect(pinByte, null, DataProtectionScope.CurrentUser);

            // return the string
            return Convert.ToBase64String(protectedBytes);
        }

        /// <summary>
        /// Decrypt a string that is stored in the phone's isolated storage in the provided path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string DecryptStringFromStorage(string path)
        {
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!file.FileExists(path)) return string.Empty;
            }
            // Retrieve the string from isolated storage.
            byte[] protectedPinByte = ReadStringFromFile(path);

            // Decrypt the string by using the Unprotect method.
            byte[] pinByte = ProtectedData.Unprotect(protectedPinByte, null, DataProtectionScope.CurrentUser);

            // Convert the PIN from byte to string and display it in the text box.
            return Encoding.UTF8.GetString(pinByte, 0, pinByte.Length);
        }

        
        public static string DecryptStringFromString(string encodedString)
        {
            
            byte[] protectedPinByte = new byte[encodedString.Length];

            // Decrypt the string by using the Unprotect method.
            byte[] pinByte = ProtectedData.Unprotect(protectedPinByte, null, DataProtectionScope.CurrentUser);

            // Convert the PIN from byte to string and display it in the text box.
            return Encoding.UTF8.GetString(pinByte, 0, pinByte.Length);
        }

        private static void WriteProtectedStringToFile(byte[] strinData, string path)
        {
            // Create a file in the application's isolated storage.
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                IsolatedStorageFileStream writestream = new IsolatedStorageFileStream(path, FileMode.Create, FileAccess.Write, file);

                // Write stringData to the file.
                Stream writer = new StreamWriter(writestream).BaseStream;
                writer.Write(strinData, 0, strinData.Length);
                writer.Close();
                writestream.Close();
            }
        }

        private static byte[] ReadStringFromFile(string path)
        {
            // Access the file in the application's isolated storage.
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                IsolatedStorageFileStream readstream = new IsolatedStorageFileStream(path, FileMode.Open, FileAccess.Read, file);

                // Read the PIN from the file.
                Stream reader = new StreamReader(readstream).BaseStream;
                byte[] pinArray = new byte[reader.Length];

                reader.Read(pinArray, 0, pinArray.Length);
                reader.Close();
                readstream.Close();

                return pinArray;
            }
        }
    }
     */
}