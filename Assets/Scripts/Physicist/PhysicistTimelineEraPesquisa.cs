using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    public static void LiberarPesquisa(PhysicistTimelineEraPesquisa pesquisa)
    {
        pesquisa.unlocked = true;
    }
    public static void LiberarObjetosPesquisa(PhysicistTimelineEraPesquisa pesquisa)
    {
        foreach (var objeto in pesquisa.objetosPesquisa.correctObjects)
        {
            objeto.unlocked = true;
        }
    }
    public static void LiberarObjetosPesquisa(List<ObjectData> listaObjetos)
    {
        foreach (var objeto in listaObjetos)
        {
            objeto.unlocked = true;
        }
    }
    public static void LiberarObjetosPesquisa(CorrectObjectList listaOBJS)
    {
        foreach (var objeto in listaOBJS.correctObjects)
        {
            objeto.unlocked = true;
        }
    }
}


