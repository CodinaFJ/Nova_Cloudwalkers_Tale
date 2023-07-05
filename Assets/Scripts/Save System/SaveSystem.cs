using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    private const string GAME_SAVE_PATH = "novaSave.bin";
    private const string CONFIG_PATH = "novaConfig.bin";

    private static string gameSavepath = Path.Combine(Application.persistentDataPath, GAME_SAVE_PATH);
    private static string configPath = Path.Combine(Application.persistentDataPath, CONFIG_PATH);

    public static string FilePath { get => gameSavepath; }

#region GameProgress

    public static void SaveGame()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(gameSavepath, FileMode.Create);  
        GameSaveData data = new GameSaveData();
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameSaveData LoadGame ()
    {
        if (File.Exists(gameSavepath))
        {
            Debug.Log(("File Exists"));
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(gameSavepath, FileMode.Open);

            GameSaveData data = formatter.Deserialize(stream) as GameSaveData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogWarning("Save file not found in " + gameSavepath);
            return null;
        }
    }

    public static void DestroySavedData()
    {
        if (File.Exists(gameSavepath))
            File.Delete(gameSavepath);
    }
    public static bool ExistsSavedGame() => File.Exists(gameSavepath);

#endregion

#region ConfigurationData

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

#endregion
}
