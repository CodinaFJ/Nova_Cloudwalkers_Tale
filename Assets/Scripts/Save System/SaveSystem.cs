using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    static string path = Path.Combine(Application.persistentDataPath, "game.bin");
    static string configPath = Path.Combine(Application.persistentDataPath, "gameConfig.bin");

    public static string FilePath { get => path; }

    /**************************************************************************************************
    Game progress
    **************************************************************************************************/

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);  
        GameSaveData data = new GameSaveData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameSaveData LoadGame ()
    {
        if (File.Exists(path))
        {
            Debug.Log(("File Exists"));
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameSaveData data = formatter.Deserialize(stream) as GameSaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + path);
            return null;
        }
    }

    public static void DestroySavedData()
    {
        if (File.Exists(path))
            File.Delete(path);
    }
    public static bool ExistsSavedGame() => File.Exists(path);

    /**************************************************************************************************
    Configuration data
    **************************************************************************************************/

    public static void SaveConfigData()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream configStream = new FileStream(configPath, FileMode.Create); 
        ConfigurationSaveData configData = new ConfigurationSaveData();
        formatter.Serialize(configStream, configData);
        configStream.Close();
    }


    public static ConfigurationSaveData LoadConfigData ()
    {
        if (File.Exists(configPath))
        {
            Debug.Log(("Config File Exists"));
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream configStream = new FileStream(configPath, FileMode.Open);
            ConfigurationSaveData configData = formatter.Deserialize(configStream) as ConfigurationSaveData;
            configStream.Close();

            return configData;
        }
        else
        {
            Debug.LogWarning("Config file not found in " + configPath);
            return null;
        }
    }

    public static void DestroyConfigData()
    {
        if (File.Exists(configPath))
            File.Delete(configPath);
    }
    public static bool ExistsConfigData() => File.Exists(configPath);
}
