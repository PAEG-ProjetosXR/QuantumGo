using System;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public List<PhysicistData> foundPhysicists = new List<PhysicistData>(); //lista pública para vermos no inspector quais físicos foram encontrados
    public List<ObjectData> foundObjects = new List<ObjectData>();
    public QuestDatabase questDatabase;
    public PhysicistDatabase physicistDatabase;
    public ObjectDatabase objectDatabase;
    public ObjpediaManager objpediaManager;
    public PhyspediaManager physpediaManager;
    public SaveSystem.SaveData currentSaveData;

    public void RegisterPhysicistEncounter(PhysicistData physicistData)  //método que outros scripts vão chamar para registrar um encontro
    {
        if (!foundPhysicists.Contains(physicistData))
        {
            foundPhysicists.Add(physicistData);
            physpediaManager.physicistCards[physicistData.id].SetFound();
            Debug.Log($"Encontrou e registrou um novo físico: {physicistData.name}!");
        }
        else
        {
            physpediaManager.physicistCards[physicistData.id].SetFoundAgain();
            Debug.Log($"{physicistData.name} já tinha sido encontrado antes.");
        }

        SaveSystem.Save(ref currentSaveData);
    }

    public void RegisterObjectEncounter(ObjectData objectData)
    {
        if (!foundObjects.Contains(objectData))
        {
            foundObjects.Add(objectData);
            objpediaManager.objectCards[objectData.id].SetFound();
            Debug.Log($"Encontrou e registrou um novo físico: {objectData.name}!");
        }
        else
        {
            Debug.Log($"{objectData.name} já tinha sido encontrado antes.");
        }

        SaveSystem.Save(ref currentSaveData);
    }

    private void ResetAllQuests()
    {
        foreach (var quest in questDatabase.allQuests)
        {
            quest.questActive = false;
            quest.questCompleted = false;
        }
    }

    private void SetSaveData()
    {
        currentSaveData.foundPhysicistsList = foundPhysicists;
        currentSaveData.foundObjectsList = foundObjects;
    }

    private void LoadSaveData()
    {
        bool loadedSuccess = SaveSystem.Load(ref currentSaveData);

        if (!loadedSuccess)
        {
            return;
        }

        foundPhysicists = currentSaveData.foundPhysicistsList;
        foundObjects = currentSaveData.foundObjectsList;
    }
    
    private void Start()
    {
        currentSaveData = new SaveSystem.SaveData();
        
        LoadSaveData();
        SetSaveData();
        ResetAllQuests();
    }
}