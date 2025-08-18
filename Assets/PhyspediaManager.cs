using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhyspediaManager : MonoBehaviour
{
    public List<PhysicistCard> physicistCards = new List<PhysicistCard>();
    private EncounterManager encounterManager;
    
    
    public void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();
        Debug.Log(encounterManager.physicistDatabase.allPhysicists.Count);
        initializePhyspedia();
    }
    
    
    
    public void initializePhyspedia()
    {
        foreach (PhysicistData data in encounterManager.physicistDatabase.allPhysicists)
        {
            // Match ID to card slot index
            if (data.id >= 0 && data.id < physicistCards.Count)
            {
                addToPhyspedia(data, physicistCards[data.id]);
            }

            if (!encounterManager.foundPhysicists.Contains(data))
            {
                physicistCards[data.id].SetUnfound();
            }
        }
    }
    
    private void addToPhyspedia(PhysicistData physicist, PhysicistCard physicistCard)
    {
        physicistCard.SetData(physicist);
    }

    
}
