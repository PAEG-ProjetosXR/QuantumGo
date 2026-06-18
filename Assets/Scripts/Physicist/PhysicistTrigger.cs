using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;
using System.Text.RegularExpressions;
using NUnit.Framework;
using System.Collections.Generic;

public class PhysicistTrigger : MonoBehaviour
{
    public PhysicistData data;
    public CaptureInfo info;   // Imagem que gerou esse objeto, NÃO fica no SO pois é própria de cada instância
    public bool foiEscolhido = false; // auxiliar para evitar muito OnDestroy sendo disparado
    public static event Action<PhysicistTrigger> OnPhysicistDestroyed;
    public int interactionCount = 0;


    [Header("Physicist Stats")]
    [SerializeField] private float hpBase; // HP base do físico
    [SerializeField] private float hpScale; // Escala do aumento de HP do físico

    private float hpMax; // HP max do físico
    private int hpCurrent; // HP atual do físico

    private EncounterManager encounterManager;

    private void Start()
    {
        encounterManager = FindAnyObjectByType<EncounterManager>();//encontra o "cerebro" na cena para poder se comunicar com ele
        CalculateHp();
    }

    private void CalculateHp() // Definir HP maximo do fisico quando ele for encontrado
    {
        if (encounterManager != null)
        {
            hpMax = hpBase + data.foundTimes * hpScale;
            hpCurrent = (int) hpMax;
            Debug.Log("Resultado: o HP max dele será " + hpCurrent + " ! (antes tava como " + hpMax + ")");
        }
        else
        {
            hpCurrent = 1;
            Debug.LogWarning("Como encounterManager = NULL, botamos vida do Físico como " + hpCurrent + " !");
        }
            
    }

    public bool ReduceHp(int ammount) // Reduzir o HP do fisico quando ele for atingido pela pokebola
    {
        hpCurrent = hpCurrent - ammount;
        Debug.Log("O alvo perdeu HP. Agora ele esta com " + hpCurrent + " de vida!");
        if (hpCurrent <= 0)
        {
            return true;
        }
        return false;
    }

    public void TriggerEncounter()  //função que sera chamada quando o jogador interagir com este objeto
    {
        if (data != null && encounterManager != null)
        {
            if (hpCurrent <= 0)
            {
                data.physicistCaptureInfo.ForEach(info => { Debug.Log(info.ToString()); });
                encounterManager.RegisterPhysicistEncounter(data);
            }
        }
    }

    private void OnDestroy()
    {
        if (foiEscolhido && info != null)
        {
            //DateTime atual = DateTime.Now;
            //int recapMod = 0;
            //recapMod = UnityEngine.Random.Range(3,6);
            //DateTime prox = atual.AddMinutes(recapMod);
            //foreach (CaptureInfo ci in data.physicistCaptureInfo)
            //    if(ci == info)
            //    {
            //        ci.captureTime = atual;
            //        ci.recaptureTime = prox;
            //    }

            OnPhysicistDestroyed?.Invoke(this);
        }
    }
}