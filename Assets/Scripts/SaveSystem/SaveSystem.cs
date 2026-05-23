using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    [System.Serializable]
    public struct SaveData
    {
        public List<PhysicistDataPack> foundPhysicistsDataPacks;
        public List<ObjectDataPack> foundObjectsDataPacks;
    }


    [System.Serializable]
    public struct PhysicistDataPack
    {
        public int id;
        public int foundTimes;
    }

    [System.Serializable]
    public struct ObjectDataPack
    {
        public int id;
        public int foundTimes;
    }

    private static string GetFilePath()
    {
        return Application.persistentDataPath + "/saveData.save"; //C:/Users/[user]/AppData/LocalLow/DefaultCompany/Quantum GO
    }

    public static void Save(ref SaveData currentSaveData)
    {
        string json = JsonUtility.ToJson(currentSaveData, true);
        File.WriteAllText(GetFilePath(), json);
    }

    public static bool Load(ref SaveData loadData)
    {
        string filePath = GetFilePath();
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            loadData = JsonUtility.FromJson<SaveData>(json);
            return true;
        }
        
        Debug.LogWarning("No save file found at " + filePath);
        return false;
        
    }
}
