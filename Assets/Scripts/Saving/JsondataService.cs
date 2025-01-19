using Newtonsoft.Json;
using System.IO;
using System;
using System.Security.Cryptography;
using UnityEngine;
using System.Text;

public class JsondataService : IDataService
{
    private const string KEY = "vZEIuXNBV4LV4Do0RcSYXLZaiXJzRyGCzIjdUXisgTk=";
    private const string IV = "r4nn1QgdQMhTfi6KAAEBiA==";

    public bool SaveData<T>(string relativePath, T data, bool encrypted)
    {
        string path = Application.persistentDataPath + relativePath;
        Debug.Log(path);
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            using FileStream stream = File.Create(path);
            if(encrypted)
            {
                WriteEncryptedData(data, stream);
            }
            else
            {
                stream.Close();
                File.WriteAllText(path, JsonConvert.SerializeObject(data));
            }
            return true;
        }
        catch (Exception)
        {
            Debug.Log("something went wrong");
            return false;
        }
    }

    private void WriteEncryptedData<T>(T data, FileStream stream)
    {
        using Aes aesProvider = Aes.Create();
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);
        using ICryptoTransform cryptoTransform = aesProvider.CreateEncryptor();
        using CryptoStream cryptoStream = new CryptoStream(
            stream,
            cryptoTransform,
            CryptoStreamMode.Write
            );

        cryptoStream.Write(Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(data)));
    }

    public bool LoadData<T>(string relativePath, bool encrypted, out T data)
    {
        string path = Application.persistentDataPath + relativePath;

        if (!File.Exists(path))
        {
            data = default;
            return false;
        }

        try
        {
            if (encrypted)
            {
                data = ReadEncryptedData<T>(path);
            }
            else
            {
               data = JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            }
            return true;
        }
        catch(Exception ex)
        {
            Debug.LogError($"failed loading data {ex.Message} {ex.StackTrace}");
            throw ex;
        }
    }

    private T ReadEncryptedData<T>(string path)
    {
        byte[] fileBytes = File.ReadAllBytes(path);
        using Aes aesProvider = Aes.Create();
        
        aesProvider.Key = Convert.FromBase64String(KEY);
        aesProvider.IV = Convert.FromBase64String(IV);

        using ICryptoTransform cryptoTransform = aesProvider.CreateDecryptor(
            aesProvider.Key,
            aesProvider.IV);

        using MemoryStream decryptionStream = new(fileBytes);

        using CryptoStream cryptoStream = new(
            decryptionStream,
            cryptoTransform,
            CryptoStreamMode.Read);

        using StreamReader reader = new(cryptoStream);

        string result = reader.ReadToEnd();

        return JsonConvert.DeserializeObject<T>(result);
    }

    public void ReadDataFromText<T>(string Text, out T Data)
    {
        Data = JsonConvert.DeserializeObject<T>(Text);
    }
}
