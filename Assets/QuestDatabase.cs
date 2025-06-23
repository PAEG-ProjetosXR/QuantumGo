using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Database", menuName = "QuantumGo/Database/Quest")]
public class QuestDatabase : ScriptableObject
{
    public List<QuestData> allQuests = new List<QuestData>();
}
