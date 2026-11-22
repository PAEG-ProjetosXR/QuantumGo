using UnityEngine;
using System;

public abstract class MinigameBase : MonoBehaviour
{
    // Finaliza a partida e informa se houve sucesso
    protected void FinishMinigame(bool success)
    {
        MinigameManager.Instance.CloseMinigame(success);
    }

    // Inicializa o minigame
    public abstract void Setup(MinigameData data);

    // Mostra o resultado da partida
    public virtual void ShowResult(bool playerWon)
    {
    }
}
