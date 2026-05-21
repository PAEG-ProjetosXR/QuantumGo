using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjpediaManager : MonoBehaviour
{
    public List<ObjectCard> objectCards = new List<ObjectCard>();
    public GameObject objectCardPrefab; // Assign in inspector: the prefab for a ObjectCard
    public GameObject objepediaContent; // Assign in inspector: the parent GameObject that holds all the ObjectCard instances
    public GameObject detailPanel; // Assign in inspector: the panel that shows detailed info about an object
    private EncounterManager encounterManager;
    
    
    public void Start()
    {
        encounterManager = FindAnyObjectByType<EncounterManager>();
        Debug.Log(encounterManager.objectDatabase.allObjects.Count);
        initializeObjspedia();
    }
    
    
    
    public void initializeObjspedia()
    {
        ObjectCard newCard = null;
        for (int i = 0; i < encounterManager.objectDatabase.allObjects.Count; i++)
        {
            // Add cards as necessary
            GameObject objectCardPrefabClone = Instantiate(objectCardPrefab, objepediaContent.transform, false);

            newCard = objectCardPrefabClone.GetComponent<ObjectCard>();

            objectCardPrefabClone.GetComponent<Button>().onClick.AddListener(newCard.OnClick);

            objectCards.Add(newCard);

        }

        foreach (ObjectData data in encounterManager.objectDatabase.allObjects)
        {
            if (data.id >= 0)
            {
                addToObjspedia(data, objectCards[data.id]);
            }

            if (!encounterManager.foundObjects.Contains(data))
            {
                objectCards[data.id].SetUnfound();
            }
        }
    }
    
    private void addToObjspedia(ObjectData objectData, ObjectCard objectCard)
    {
        objectCard.SetData(objectData);
    }
}
