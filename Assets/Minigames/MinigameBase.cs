using UnityEngine;
using System;

//Padroniza a forma como qualquer minigame é iniciado/finalizado
public abstract class MinigameBase : MonoBehaviour
{
    //Método de finalização
    protected void FinishMinigame(bool sucess) { MinigameManager.Instance.CloseMinigame(sucess); }

    //Método de inicialização
    public abstract void Setup(MinigameData data);
}
