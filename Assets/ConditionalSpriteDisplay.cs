using UnityEngine;
using UnityEngine.UI; // Adicione se o seu sprite for um componente Image da UI

public class ConditionalSpriteDisplay : MonoBehaviour
{
    [Header("Referências e Configurações")]
    public GameObject spriteGameObject; // Arraste o GameObject do seu sprite aqui

    // Escolha UMA das opções abaixo para definir o pré-requisito
    public PhysicistData requiredPhysicist;
    public ObjectData requiredObject;

    private EncounterManager encounterManager; // Referência para o EncounterManager

    void Awake() // Usar Awake pode ser melhor para encontrar referências antes do Start de outros
    {
        // Encontra o EncounterManager na cena, similar ao seu PhysicistTrigger
        encounterManager = FindObjectOfType<EncounterManager>();

        if (encounterManager == null)
        {
            Debug.LogError("ConditionalSpriteDisplay: EncounterManager não encontrado na cena!");
            // Opcionalmente, desabilite o sprite se o manager não for encontrado para evitar erros
            if (spriteGameObject != null)
            {
                spriteGameObject.SetActive(false);
            }
        }
    }

    void OnEnable() // Chamado quando o GameObject se torna ativo e sempre que é reativado
    {
        UpdateSpriteVisibility();
    }

    public void UpdateSpriteVisibility()
    {
        if (encounterManager == null)
        {
            // Se o encounterManager não foi encontrado no Awake, não faz nada ou garante que está escondido
            if (spriteGameObject != null) spriteGameObject.SetActive(false);
            return;
        }

        if (spriteGameObject == null)
        {
            Debug.LogError("ConditionalSpriteDisplay: Sprite GameObject não está atribuído!");
            return;
        }

        if (requiredPhysicist == null && requiredObject == null)
        {
            Debug.LogWarning("ConditionalSpriteDisplay: Nenhum físico ou objeto requerido foi atribuído. O sprite será mostrado por padrão.");
            spriteGameObject.SetActive(true); // Comportamento padrão: mostrar se nada for exigido
            return;
        }

        bool hasBeenEncountered = false;

        if (requiredPhysicist != null)
        {
            hasBeenEncountered = encounterManager.foundPhysicists.Contains(requiredPhysicist);
        }
        else if (requiredObject != null)
        {
            hasBeenEncountered = encounterManager.foundObjects.Contains(requiredObject);
        }

        spriteGameObject.SetActive(hasBeenEncountered);

        // Log para depuração
        string itemName = requiredPhysicist != null ? requiredPhysicist.name : (requiredObject != null ? requiredObject.name : "N/A");
        //Debug.Log($"Sprite {spriteGameObject.name} para {itemName} está {(hasBeenEncountered ? "visível" : "oculto")}");
    }
}