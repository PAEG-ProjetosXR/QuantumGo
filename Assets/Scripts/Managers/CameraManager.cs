using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // A "instância" estática permite que acessemos este script de qualquer outro lugar
    // usando CameraManager.Instance
    public static CameraManager Instance { get; private set; }

    // Campo público para arrastarmos a referência no Inspector
    public Transform PokeBallHoldPosition;
    public Transform PlayerInputAnchor; // Referência para o nosso novo âncora

    private void Awake()
    {
        // Configuração do padrão Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
}