using UnityEngine;

public class IniciarMinigameBtn : MonoBehaviour
{
    public MinigameData testeMinigameData;
    public MinigameManager minigameManager;

    public void OnClick()
    {
        minigameManager.StartMinigame(testeMinigameData);
    }
}
