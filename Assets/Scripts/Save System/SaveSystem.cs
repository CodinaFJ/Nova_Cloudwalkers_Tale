using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem 
{
    static string path = Path.Combine(Application.persistentDataPath, "game.bin");

    public static string FilePath { get => path; }

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

}
