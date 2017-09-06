using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class ApplicationDatas
{
    public const int MAX_LEVEL = 9;
    public static GameDatas Game = new GameDatas();

    public static bool HasSave()
    {
        return (File.Exists(Application.persistentDataPath + "/ColorWarrior.save"));
    }

    public static void NewGame()
    {
        Game = new GameDatas();
    }

    public static void Save()
    {
        FileStream slot = File.Create(Application.persistentDataPath + "/ColorWarrior.save");
        new BinaryFormatter().Serialize(slot, Game);
        slot.Close();
    }

    public static void Load()
    {
        if (HasSave())
        {
            FileStream file = File.Open(Application.persistentDataPath + "/ColorWarrior.save", FileMode.Open);
            Game = (GameDatas)(new BinaryFormatter().Deserialize(file));
            file.Close();
        }
    }

    public static void Clear()
    {
        if (HasSave())
        {
            File.Delete(Application.persistentDataPath + "/ColorWarrior.save");
        }
    }
}
