using System.Collections.Generic;
using UnityEngine;

public class EncounterManager : MonoBehaviour
{
    public List<PhysicistData> foundPhysicists = new List<PhysicistData>(); //lista pública para vermos no inspector quais físicos foram encontrados

    public void RegisterEncounter(PhysicistData physicist)  //método que outros scripts vão chamar para registrar um encontro
    {
        if (!foundPhysicists.Contains(physicist))
        {
            foundPhysicists.Add(physicist);
            Debug.Log($"Encontrou e registrou um novo físico: {physicist.physicistName}!");
        }
        else
        {
            Debug.Log($"{physicist.physicistName} já tinha sido encontrado antes.");
        }
    }
}