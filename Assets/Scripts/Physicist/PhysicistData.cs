using Fog.Dialogue;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[CreateAssetMenu(fileName = "New Physicist", menuName = "QuantumGo/Physicist")]     //opção no menu para criar novos Físicos no editor 
public class PhysicistData : ScriptableObject
{
    public int id;
    public string name;
    [TextArea(3, 10)]
    public string description;                      //Dividir em partes para ser lentamente descoberto
    public Dialogue dialogue;
    public QuestData quest;
    
    public Sprite icon;                              // icone para a enciclopédia
    public GameObject modelPrefab;                   //O modelo 3D do físico
    public AudioClip presentationAudio;              //O áudio da apresentação
    public ObjectData correctIcon;                   // Correlação Feito/fisico

    public int waitRecaptureSecs;

    [NonSerialized]
    public int foundTimes = 0;                       // Indica se o físico foi encontrado ou não

    [NonSerialized]
    // CHECAR NULO ANTES DE USAR PARAMETROS DA CLASSE CaptureInfo
    public List<CaptureInfo> physicistCaptureInfo;   // Imagens capturáveis do físico e seus correspondentes modelos e datas de captura

}

public class CaptureInfo
{
    public ARTrackedImage trackedImage;
    public GameObject model;
    public DateTime? captureTime;
    public DateTime? recaptureTime;
    public CaptureInfo(ARTrackedImage trackedImage, GameObject model, DateTime? captureTime, DateTime? recaptureTime = null)
    {
        this.trackedImage = trackedImage;
        this.model = model;
        this.captureTime = captureTime;
        this.recaptureTime = recaptureTime;
    }

    public override string ToString()
    {
        string imageName = trackedImage == null ? "NULL" : trackedImage.name;
        string modelName = model == null ? "NULL" : model.name;

        return
            $"CaptureInfo\n" +
            $"  Tracked Image: {imageName}\n" +
            $"  Model:         {modelName}\n" +
            $"  Capture Time:  {captureTime:yyyy-MM-dd HH:mm:ss}" +
            $"  Recapture Time:  {recaptureTime:yyyy-MM-dd HH:mm:ss}";
    }
}