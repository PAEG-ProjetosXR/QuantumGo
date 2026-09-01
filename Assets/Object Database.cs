using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Database", menuName = "QuantumGo/Database/Object")]
public class ObjectDatabase : ScriptableObject
{
    public List<ObjectData> allObjects = new List<ObjectData>();
}
