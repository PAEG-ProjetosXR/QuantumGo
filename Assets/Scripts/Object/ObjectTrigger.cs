using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    public ObjectData data;
    
    private EncounterManager encounterManager;

    void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();    //encontra o "cerebro" na cena para poder se comunicar com ele
    }

    public void TriggerEncounter()  //função que sera chamada quando o jogador interagir com este objeto

    {
        if (data != null && encounterManager != null)
        {
            encounterManager.RegisterObjectEncounter(data);
        }
    }
}
