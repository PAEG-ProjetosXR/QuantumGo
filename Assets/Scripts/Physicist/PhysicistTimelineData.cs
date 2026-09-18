using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicistTimelineData", menuName = "Scriptable Objects/PhysicistTimelineData")]
public class PhysicistTimelineData : ScriptableObject
{
    public PhysicistData physicistData;
    public List<TimelineEra> listaErasTimeline;
}

public class TimelineEra
{
    Color corDivisoria;
    DateTime anoInicio;
    DateTime anoFim;
    string titulo;
    List<TimelineEraObject> listaObjetos;

}

public class TimelineEraObject
{
    public ObjectData objectData;
    public DateTime data;
}
