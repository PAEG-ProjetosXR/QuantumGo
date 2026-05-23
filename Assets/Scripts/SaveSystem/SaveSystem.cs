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

    private static string saveFilePath = Application.persistentDataPath + "/saveData.save";


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

    public static void Save(ref SaveData currentSaveData)
    {
        string json = JsonUtility.ToJson(currentSaveData, true);
        File.WriteAllText(saveFilePath, json);
    }

    public static bool Load(ref SaveData loadData)
    {
        Debug.Log("Caminho do arquivo de save: " + saveFilePath);
        string filePath = saveFilePath;
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
