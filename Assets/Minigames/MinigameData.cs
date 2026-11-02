using UnityEngine;

//Classe base de configuração para qualquer minigame
public abstract class MinigameData : ScriptableObject
{
    [Header("Geral")]
    public string minigameName;
    [TextArea] public string minigameDescription;
    public float timeLimit = 30f;
    public int difficulty = 1;
    public GameObject minigameUIPrefab; //O prefab da UI do minigame
}
