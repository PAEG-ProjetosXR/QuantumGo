using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicistTimelineEra", menuName = "Scriptable Objects/PhysicistTimelineEra")]
public class PhysicistTimelineEra : ScriptableObject
{

    public Color corDivisoria;
    public int anoInicio;
    public int anoFim;
    public string titulo;
    public List<PhysicistTimelineEraObject> listaObjetos;
    
}
