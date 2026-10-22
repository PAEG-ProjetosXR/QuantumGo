using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CorrectObjectList", menuName = "Scriptable Objects/CorrectObjectList")]
public class CorrectObjectList : ScriptableObject
{
     public List<ObjectData> correctObjects; //os objetos, em ordem, que é resposta
}
