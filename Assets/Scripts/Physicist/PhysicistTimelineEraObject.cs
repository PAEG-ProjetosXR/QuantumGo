using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PhysicistTimelineEraObject", menuName = "Scriptable Objects/PhysicistTimelineEraObject")]
public class PhysicistTimelineEraObject : ScriptableObject
{
    public ObjectData objectData;
    public DateTime data;
}
