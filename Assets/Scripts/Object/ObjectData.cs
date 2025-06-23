using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "QuantumGo/Object")]
public class ObjectData : ScriptableObject
{
    public int id;
    public string name;
    [TextArea(3, 10)]
    public string description;
    
    public Sprite icon;                              // icone para a enciclopédia
    public GameObject modelPrefab;                   //O modelo 3D do físico
    public AudioClip presentationAudio;              //O áudio da apresentação
}
