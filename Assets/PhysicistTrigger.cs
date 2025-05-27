using UnityEngine;

public class PhysicistTrigger : MonoBehaviour
{
    public PhysicistData physicistToTrigger;
    
    private EncounterManager encounterManager;

    void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();    //encontra o "cerebro" na cena para poder se comunicar com ele
    }

    public void TriggerEncounter()  //função que sera chamada quando o jogador interagir com este objeto

    {
        if (physicistToTrigger != null && encounterManager != null)
        {
            encounterManager.RegisterEncounter(physicistToTrigger);
        }
    }
}