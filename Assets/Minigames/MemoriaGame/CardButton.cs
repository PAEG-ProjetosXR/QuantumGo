using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    [Header("Referências Visuais")]
    [SerializeField] private Image cardImage; // O componente Image que vai exibir a frente ou o verso
    [SerializeField] private Button button;

    [Header("Sprites das Cartas")]
    private Sprite frontSprite;
    [SerializeField] private Sprite backSprite;

    private int cardID;
    private MemoriaGameUI gameManager;

    private bool isRevealed = false;
    private bool isMatched = false;

    public void SetupCard(int id, Sprite cardSprite, MemoriaGameUI manager)
    {
        cardID = id;
        frontSprite = cardSprite;
        gameManager = manager;

        if (button == null) button = GetComponent<Button>();
        if (cardImage == null) cardImage = GetComponent<Image>();

        // O jogo começa com todas as cartas viradas para baixo (escondidas)
        HideCard();

        // Evita duplicar o listener caso o Setup seja chamado novamente
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);
    }

    private void OnClickCard()
    {
        // Se a carta já estiver revelada ou já tiver feito par, ignora o clique
        if (isRevealed || isMatched) return;

        // Revela a carta visualmente
        RevealCard();

        // Avisa o script principal que esta carta foi clicada
        gameManager.OnCardClicked(this);
    }

    public void RevealCard()
    {
        isRevealed = true;
        cardImage.sprite = frontSprite;
    }

    public void HideCard()
    {
        if (isMatched) return; // Se já achou o par, não esconde mais

        isRevealed = false;
        cardImage.sprite = backSprite;
    }

    public void SetMatched()
    {
        isMatched = true;
        button.interactable = false; // Desativa o botão para o jogador não clicar mais nele
    }

    public int GetCardID()
    {
        return cardID;
    }
}