using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[CreateAssetMenu(fileName = "New Object", menuName = "QuantumGo/Object")]
public class ObjectData : ScriptableObject
{
    public bool unlocked;
    public int id;
    public string name;
    [TextArea(3, 10)]
    public string description;
    
    public Sprite icon;                              // icone para a enciclopédia
    public GameObject modelPrefab;                   //O modelo 3D do objeto
    public AudioClip presentationAudio;              //O áudio da apresentação

    public int waitRecaptureSecs;

    [NonSerialized]
    public int foundTimes = 0;                       // Indica se o objeto foi encontrado ou não

    [NonSerialized]
    // CHECAR NULO ANTES DE USAR PARAMETROS DA CLASSE CaptureInfo
    public List<CaptureInfo> objectCaptureInfo;   // Imagens capturáveis do objeto e seus correspondentes modelos e datas de captura

    // as duas funções abaixo são "overrides" que permitem comparar 2 ObjectData segundo id, útil para comparar lista
    public override bool Equals(object obj)
    {
        if (obj is not ObjectData other)
            return false;

        return id == other.id;
    }

    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}
