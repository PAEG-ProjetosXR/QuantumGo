using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[CreateAssetMenu(fileName = "New Object", menuName = "QuantumGo/Object")]
public class ObjectData : ScriptableObject
{
    public int id;
    public string name;
    [TextArea(3, 10)]
    public string description;
    
    public Sprite icon;                              // icone para a enciclopédia
    public GameObject modelPrefab;                   //O modelo 3D do objeto
    public AudioClip presentationAudio;              //O áudio da apresentação

    [NonSerialized]
    public int foundTimes = 0;                       // Indica se o objeto foi encontrado ou não

    [NonSerialized]
    // CHECAR NULO ANTES DE USAR PARAMETROS DA CLASSE CaptureInfo
    public List<CaptureInfo> objectCaptureInfo;   // Imagens capturáveis do objeto e seus correspondentes modelos e datas de captura
}
