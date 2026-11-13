using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Object Database", menuName = "QuantumGo/Database/Object")]
public class ObjectDatabase : ScriptableObject
{
    [NonSerialized] //precisa disso pra unity não salvar permanentemente quando os scripts modificam a lista (algo q n aconteceria na build real)
    public List<ObjectData> allObjects = new List<ObjectData>();
}
