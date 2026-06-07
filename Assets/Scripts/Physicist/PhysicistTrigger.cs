using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;

public class PhysicistTrigger : MonoBehaviour
{
    public PhysicistData data;
    public CaptureInfo info;   // Imagem que gerou esse objeto, NÃO fica no SO pois é própria de cada instância
    public bool foiEscolhido = false; // auxiliar para evitar muito OnDestroy sendo disparado
    public int interactionCount = 0;
    
    private EncounterManager encounterManager;

    void Start()
    {
        encounterManager = FindAnyObjectByType<EncounterManager>();    //encontra o "cerebro" na cena para poder se comunicar com ele
    }

    public void TriggerEncounter()  //função que sera chamada quando o jogador interagir com este objeto

    {
        if (data != null && encounterManager != null)
        {
            data.physicistCaptureInfo.ForEach(info => { Debug.Log(info.ToString()); });
            encounterManager.RegisterPhysicistEncounter(data);
        }
    }

    private void OnDestroy()
    {
        if (foiEscolhido && info != null)
        {
            DateTime atual = DateTime.Now;
            info.captureTime = atual;
            int recapMod = 0;
            recapMod = UnityEngine.Random.Range(3,6);
            info.recaptureTime = atual.AddMinutes(recapMod);
        }
    }
}