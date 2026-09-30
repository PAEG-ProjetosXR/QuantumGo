using System;
using UnityEngine;

//Script será anexado num objeto fixo na cena e controla a criação/execução/destruição dos minigames
public class MinigameManager : MonoBehaviour
{
    public static MinigameManager Instance {  get; private set; }

    [SerializeField] private Transform canvasOverlay; //onde vai ocorrer o spawn da UI do minigame

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(Instance); }
    }

    //Chama qualquer minigame e avisa o objeto solicitante se o jogador venceu
    public void StartMinigame(MinigameData config, Action<bool> onComplete)
    {
        //Instancia a UI do minigame no Canvas
        GameObject instance = Instantiate(config.minigameUIPrefab, canvasOverlay);
        MinigameBase minigameScript = instance.GetComponent<MinigameBase>();

        //Passa os dados do ScriptableObject
        minigameScript.Setup(config);

        //Escuta a conclusão para destruir a UI e avisar quem chamou
        minigameScript.OnMinigameEnded += (success) =>
        {
            Destroy(instance);
            onComplete?.Invoke(success);
        };
    }
}
