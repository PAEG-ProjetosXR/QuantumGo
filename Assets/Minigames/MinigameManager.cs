using System;
using System.Collections.Generic;
using UnityEngine;

//Controla a criação/execução/destruição dos minigames
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance {  get; private set; }

    [SerializeField]
    private SpawnManager SpawnManager;

    [Header("Configuração de Tela")]
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
        SpawnManager.GanhouMinigameAtual(); //REMOVER, só coloquei pq n tem como ganhar minigames pra testar
        // Instancia o prefab de UI do minijogo apontado no ScriptableObject, dentro do container
        GameObject prefabToSpawn = data.minigameUIPrefab;
        currentMinigameInstance = Instantiate(prefabToSpawn, minigameContainer);

        // Pega o script base do minigame e inicializa enviando os dados
        MinigameBase minigameScript = currentMinigameInstance.GetComponent<MinigameBase>();
        if (minigameScript != null) { minigameScript.Setup(data); }
        else { Debug.LogError("O prefab instanciado não possui um script que herda de MinigameBase!"); }
    }

    // Método chamado quando o minigame acaba (vitória ou derrota)
    public void CloseMinigame(bool playerWon)
    {
        if (currentMinigameInstance != null)
        {
            Destroy(currentMinigameInstance);
            currentMinigameInstance = null;
        }

        // Aqui você pode dar feedback ao restante do jogo em AR (dar recompensa se venceu, etc.)
        Debug.Log("Minijogo encerrado. Jogador venceu? " + playerWon);
        if (playerWon)
        {
            SpawnManager.GanhouMinigameAtual();
        }

    }
}
