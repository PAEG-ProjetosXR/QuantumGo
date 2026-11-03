using UnityEngine;

[CreateAssetMenu(fileName = "NovoJogoMemoria", menuName = "Minigames/Jogo da Memória")]
public class MemoriaGameData : MinigameData
{
    [Header("Jogo da Memória")]
    public int maxAttempts = 10;
    public int totalPairs = 4;
    public Sprite[] cardSprites;
}
