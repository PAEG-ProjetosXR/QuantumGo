using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    [Header("Refer�ncias Visuais")]
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

        // O jogo come�a com todas as cartas viradas para baixo (escondidas)
        HideCard();

        // Evita duplicar o listener caso o Setup seja chamado novamente
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);
    }

    private void OnClickCard()
    {
        if (isRevealed || isMatched) return;

        gameManager.OnCardClicked(this);
    }

    public void RevealCard()
    {
        isRevealed = true;
        cardImage.sprite = frontSprite;
    }

    public void HideCard()
    {
        if (isMatched) return; // Se j� achou o par, n�o esconde mais

        isRevealed = false;
        cardImage.sprite = backSprite;
    }

    public void SetMatched()
    {
        isMatched = true;
        button.interactable = false; // Desativa o bot�o para o jogador n�o clicar mais nele
    }

    public int GetCardID()
    {
        return cardID;
    }
}