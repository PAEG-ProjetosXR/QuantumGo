using System;
using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public List<PhysicistData> foundPhysicists = new List<PhysicistData>(); //lista pública para vermos no inspector quais físicos foram encontrados
    public List<ObjectData> foundObjects = new List<ObjectData>();
    public List<QuestData> allQuests = new List<QuestData>();
    
    public void RegisterPhysicistEncounter(PhysicistData physicistData)  //método que outros scripts vão chamar para registrar um encontro
    {
        if (!foundPhysicists.Contains(physicistData))
        {
            foundPhysicists.Add(physicistData);
            Debug.Log($"Encontrou e registrou um novo físico: {physicistData.name}!");
        }
        else
        {
            Debug.Log($"{physicistData.name} já tinha sido encontrado antes.");
        }
    }

    public void RegisterObjectEncounter(ObjectData objectData)
    {
        if (!foundObjects.Contains(objectData))
        {
            foundObjects.Add(objectData);
            Debug.Log($"Encontrou e registrou um novo físico: {objectData.name}!");
        }
        else
        {
            Debug.Log($"{objectData.name} já tinha sido encontrado antes.");
        }
    }

    private void ResetAllQuests()
    {
        foreach (var quest in allQuests)
        {
            quest.questActive = false;
            quest.questCompleted = false;
        }
    }

    private void Start()
    {
        ResetAllQuests();
    }
}