using System;
using System.Collections.Generic;
using UnityEngine;

//Controla a cria��o/execu��o/destrui��o dos minigames
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance {  get; private set; }

    [SerializeField]
    private SpawnManager SpawnManager;
    [SerializeField] private UIHandler uiHandler;

    [Header("Configura��o de Tela")]
    [SerializeField] private Transform minigameContainer; // Onde a UI do minigame vai nascer

    private GameObject currentMinigameInstance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    //Chama o minigame
    public void StartMinigame(MinigameData data)
    {
        if (currentMinigameInstance != null)
        {
            Debug.LogWarning("Já existe um minigame ativo!");
            return;
        }

        uiHandler.HideUIForMinigame();

        GameObject prefabToSpawn = data.minigameUIPrefab;

        currentMinigameInstance = Instantiate(
            prefabToSpawn,
            minigameContainer
        );

        MinigameBase minigameScript =
            currentMinigameInstance.GetComponent<MinigameBase>();

        if (minigameScript != null)
        {
            minigameScript.Setup(data);
        }
        else
        {
            Debug.LogError(
                "O prefab instanciado não possui MinigameBase!"
            );
        }
    }

    // M�todo chamado quando o minigame acaba (vit�ria ou derrota)
    public void CloseMinigame(bool playerWon)
    {
        if (currentMinigameInstance == null)
            return;

        MinigameBase minigame =
            currentMinigameInstance.GetComponent<MinigameBase>();

        if (minigame != null)
        {
            minigame.ShowResult(playerWon);
        }

        Debug.Log("Minijogo encerrado. Jogador venceu? " + playerWon);

        if (playerWon)
        {
            SpawnManager.GanhouMinigameAtual();
        }
    }

    public void CloseResultPanel()
    {
        if (currentMinigameInstance != null)
        {
            Destroy(currentMinigameInstance);
            currentMinigameInstance = null;
        }

        uiHandler.ShowUIAfterMinigame();
    }

    public bool IsMinigameActive
    {
        get { return currentMinigameInstance != null; }
    }
}
