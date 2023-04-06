using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Security.Cryptography;
using System.IO;

namespace Histhack.Core.SaveLoadSystem
{
    public class Encrypter
    {
        string savingKeyString = "ForrestKey";
        string savingIVString = "ForrestIV";

        private Aes aes;
        private byte[] encryptionKey;
        private byte[] iVKey;

        public void Initialize()
        {
            aes = Aes.Create();
            if (encryptionKey == null)
            {
                if (PlayerPrefs.HasKey(savingKeyString))
                {
                    string encryptionKeyString = PlayerPrefs.GetString(savingKeyString);
                    string iVKeyString = PlayerPrefs.GetString(savingIVString);

                    encryptionKey = System.Convert.FromBase64String(encryptionKeyString);
                    iVKey = System.Convert.FromBase64String(iVKeyString);
                }
                else
                {
                    encryptionKey = aes.Key;
                    string key = System.Convert.ToBase64String(encryptionKey);

                    iVKey = aes.IV;
                    string IV = System.Convert.ToBase64String(iVKey);

                    PlayerPrefs.SetString(savingKeyString, key);
                    PlayerPrefs.SetString(savingIVString, IV);
                }
            }
        }

        public CryptoStream CreateDecryptoCryptoStream(FileStream fileStream, CryptoStreamMode streamMode)
        {
            return new CryptoStream(
                    fileStream,
                    aes.CreateDecryptor(encryptionKey, iVKey),
                    streamMode
            );
        }

        public CryptoStream CreateEncryptoCryptoStream(FileStream fileStream, CryptoStreamMode streamMode)
        {
            return new CryptoStream(
                    fileStream,
                    aes.CreateEncryptor(encryptionKey, iVKey),
                    streamMode
            );
        }

    }
}
