using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicistTimelineData", menuName = "Scriptable Objects/PhysicistTimelineData")]
public class PhysicistTimelineData : ScriptableObject
{
    public PhysicistData physicistData;
    public List<PhysicistTimelineEra> listaErasTimeline;
}
