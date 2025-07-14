using Fog.Dialogue;
using UnityEngine;

[CreateAssetMenu(fileName = "New Physicist", menuName = "QuantumGo/Physicist")]     //opção no menu para criar novos Físicos no editor 
public class PhysicistData : ScriptableObject
{
    public int id;
    public string name;
    [TextArea(3, 10)]
    public string description;
    public Dialogue dialogue;
    public QuestData quest;
    
    public Sprite icon;                              // icone para a enciclopédia
    public GameObject modelPrefab;                   //O modelo 3D do físico
    public AudioClip presentationAudio;              //O áudio da apresentação
    public ObjectData correctIcon;                   // Correlação Feito/fisico

}