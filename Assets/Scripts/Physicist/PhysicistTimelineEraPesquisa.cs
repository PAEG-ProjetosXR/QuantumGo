using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PhysicistTimelineEraPesquisa", menuName = "Scriptable Objects/PhysicistTimelineEraPesquisa")]
public class PhysicistTimelineEraPesquisa : ScriptableObject
{
    public CorrectObjectList objetosPesquisa;
    public int ano;
    public Sprite icone;
    [TextArea(3,10)]
    public string desc;
    public string titulo;
    public PhysicistTimelineEraPesquisa trabalhoRelacionado;
    public PhysicistData fisicoInventor;
    public bool unlocked;
}
