using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhyspediaManager : MonoBehaviour
{
    public List<PhysicistCard> physicistCards = new List<PhysicistCard>();
    public GameObject physicistCardPrefab; // Assign in inspector: the prefab for a PhysicistCard
    public GameObject physipediaContent; // Assign in inspector: the parent GameObject that holds all the PhysicistCard instances
    private EncounterManager encounterManager;

    public void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();
        Debug.Log(encounterManager.physicistDatabase.allPhysicists.Count);
        initializePhyspedia();
    }
    
    
    
    public void initializePhyspedia()
    {
        PhysicistCard newCard = null;
        for(int i = 0; i < encounterManager.physicistDatabase.allPhysicists.Count; i++)
        {
            // Add cards as necessary
            GameObject physicistCardPrefabClone = Instantiate(physicistCardPrefab, physipediaContent.transform, false);

            newCard = physicistCardPrefabClone.GetComponent<PhysicistCard>();

            physicistCardPrefabClone.GetComponent<Button>().onClick.AddListener(newCard.OnClick);
            
            physicistCards.Add(newCard);
            
        }

        foreach (PhysicistData data in encounterManager.physicistDatabase.allPhysicists)
        {
            if (data.id >= 0)
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
