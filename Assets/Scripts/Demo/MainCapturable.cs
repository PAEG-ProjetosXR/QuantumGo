using UnityEngine;
using Fog.Dialogue;
public class MainCapturable : MonoBehaviour
{
    //Info do físico ou objeto
    public bool isObject; //indica se é físico ou objeto
    public int id;
    public string nome;
    [TextArea(3, 10)]
    public string description;                      //Dividir em partes para ser lentamente descoberto
    public Dialogue dialogue;
    public QuestData quest;
    
    public Sprite icon;                              // icone para a enciclopédia
    public GameObject modelPrefab;                   //O modelo 3D do físico
    public AudioClip presentationAudio;              //O áudio da apresentação
    public ObjectData correctIcon;
}
