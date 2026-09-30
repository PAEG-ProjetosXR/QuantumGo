using UnityEngine;
using System;

//Script será anexado ao Prefab da UI do minigame; ele padroniza a forma como qualquer minigame é iniciado/finalizado
public abstract class MinigameBase : MonoBehaviour
{
    //Evento disparado no fim: vitória (true) ou derrota (false)
    public event Action<bool> OnMinigameEnded;

    protected void FinishMinigame(bool sucess)
    {
        OnMinigameEnded?.Invoke(sucess);
    }

    //Método de inicialização
    public abstract void Setup(MinigameData data);
}
