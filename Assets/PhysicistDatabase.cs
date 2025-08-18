using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Physicist Database", menuName = "QuantumGo/Database/Physicist")]
public class PhysicistDatabase : ScriptableObject
{
    public List<PhysicistData> allPhysicists = new List<PhysicistData>();
}
