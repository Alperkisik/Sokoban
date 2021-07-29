using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveLevel(GameDatas gameDatas)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/LevelData.save";

        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream,gameDatas.level);
        stream.Close();
    }

    public static int LoadGameDatas()
    {
        string path = Application.persistentDataPath + "/LevelData.save";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            int level = (int)formatter.Deserialize(stream);

            return level;
        }
        else
        {
            return 0;
        }
    }
}
