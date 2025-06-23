using System.Collections.Generic;
using UnityEngine;

public class ObjpediaManager : MonoBehaviour
{
    public List<ObjectCard> objectCards = new List<ObjectCard>();
    private EncounterManager encounterManager;
    
    
    public void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();
        Debug.Log(encounterManager.objectDatabase.allObjects.Count);
        initializeObjspedia();
    }
    
    
    
    public void initializeObjspedia()
    {
        foreach (ObjectData data in encounterManager.objectDatabase.allObjects)
        {
            // Match ID to card slot index
            if (data.id >= 0 && data.id < objectCards.Count)
            {
                addToObjspedia(data, objectCards[data.id]);
            }
        }
    }
    
    private void addToObjspedia(ObjectData physicist, ObjectCard physicistCard)
    {
        physicistCard.SetData(physicist);
    }
}
