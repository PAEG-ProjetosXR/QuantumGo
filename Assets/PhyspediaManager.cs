using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhyspediaManager : MonoBehaviour
{
    public List<PhysicistTimeline> physicistTimelines = new List<PhysicistTimeline>();
    public GameObject physicistTimelinePrefab; // Assign in inspector: the prefab for a PhysicistTimeline
    public GameObject physipediaContent; // Assign in inspector: the parent GameObject that holds all the PhysicistTimeline instances
    private EncounterManager encounterManager;

    public void Start()
    {
        encounterManager = FindAnyObjectByType<EncounterManager>();
        initializePhyspedia();
    }
    
    public void initializePhyspedia()
    {
        PhysicistTimeline newTimeline = null;
        for(int i = 0; i < encounterManager.physicistDatabase.allPhysicists.Count; i++)
        {
            // Add cards as necessary
            GameObject PhysicistTimePrefabClone = Instantiate(physicistTimelinePrefab, physipediaContent.transform, false);

            newTimeline = PhysicistTimePrefabClone.GetComponent<PhysicistTimeline>();

            newTimeline.physicistBtn.onClick.AddListener(newTimeline.OnClick);
            
            physicistTimelines.Add(newTimeline);
            
        }

        foreach (PhysicistData data in encounterManager.physicistDatabase.allPhysicists)
        {
            if (data.id >= 0)
            {
                addToPhyspedia(data, physicistTimelines[data.id]);
            }

            if (!encounterManager.foundPhysicists.Contains(data))
            {
                physicistTimelines[data.id].SetUnfound();
            }
        }
    }
    
    private void addToPhyspedia(PhysicistData physicist, PhysicistTimeline PhysicistTimeline)
    {
        PhysicistTimeline.SetData(physicist);
    }

    
}
