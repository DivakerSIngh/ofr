using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Microsoft.SqlServer.Server;
namespace Encription
{

    public static class AESEncryption
    {

        public static string DecryptData(string PlainText,string KeyData)
        {
            try
            {
                return Decrypt(PlainText, "OneFineRate", "8", "SHA1", 2, KeyData.Trim(), 256);
            }
            catch (Exception E1)
            {
                return "Nil";
            }

        }
        public static string EncryptData(string PlainText, string KeyData)
        {
            try
            {
                return Encrypt(PlainText, "OneFineRate", "8", "SHA1", 2, KeyData.Trim(), 256);
            }
            catch (Exception E1)
            {
                return "Nil";
            }

        }
        public static string Encrypt(string PlainText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] PlainTextBytes = Encoding.UTF8.GetBytes(PlainText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform Encryptor = SymmetricKey.CreateEncryptor(KeyBytes, InitialVectorBytes);
            MemoryStream MemStream = new MemoryStream();
            CryptoStream CryptoStream = new CryptoStream(MemStream, Encryptor, CryptoStreamMode.Write);
            CryptoStream.Write(PlainTextBytes, 0, PlainTextBytes.Length);
            CryptoStream.FlushFinalBlock();
            byte[] CipherTextBytes = MemStream.ToArray();
            MemStream.Close();
            CryptoStream.Close();
            return Convert.ToBase64String(CipherTextBytes);
        }
        public static string Decrypt(string CipherText, string Password, string Salt, string HashAlgorithm, int PasswordIterations, string InitialVector, int KeySize)
        {
            byte[] InitialVectorBytes = Encoding.ASCII.GetBytes(InitialVector);
            byte[] SaltValueBytes = Encoding.ASCII.GetBytes(Salt);
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            PasswordDeriveBytes DerivedPassword = new PasswordDeriveBytes(Password, SaltValueBytes, HashAlgorithm, PasswordIterations);
            byte[] KeyBytes = DerivedPassword.GetBytes(KeySize / 8);
            RijndaelManaged SymmetricKey = new RijndaelManaged();
            SymmetricKey.Mode = CipherMode.CBC;
            ICryptoTransform Decryptor = SymmetricKey.CreateDecryptor(KeyBytes, InitialVectorBytes);
            MemoryStream MemStream = new MemoryStream(CipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(MemStream, Decryptor, CryptoStreamMode.Read);
            byte[] PlainTextBytes = new byte[CipherTextBytes.Length];
            int ByteCount = cryptoStream.Read(PlainTextBytes, 0, PlainTextBytes.Length);
            MemStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(PlainTextBytes, 0, ByteCount);
        }
    }


}
