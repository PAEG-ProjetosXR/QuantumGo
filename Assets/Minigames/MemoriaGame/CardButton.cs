using UnityEngine;
using UnityEngine.UI;

public class CardButton : MonoBehaviour
{
    private Button button;
    private Image iconImage; // O ícone da frente da carta (filho do botão)
    private int cardID;
    private MemoriaGameUI gameManager;

    private Sprite frontSprite;
    private bool isRevealed = false;
    private bool isMatched = false;

    public void SetupCard(int id, Sprite cardSprite, MemoriaGameUI manager)
    {
        cardID = id;
        frontSprite = cardSprite;
        gameManager = manager;

        button = GetComponent<Button>();
        iconImage = transform.GetChild(0).GetComponent<Image>(); // Assume que o ícone está no primeiro filho

        // O jogo começa com todas as cartas viradas para baixo (escondidas)
        HideCard();

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
        iconImage.gameObject.SetActive(true); // Mostra o ícone da carta
        iconImage.sprite = frontSprite;       // Garante que o sprite está correto
    }

    public void HideCard()
    {
        if (isMatched) return; // Se já achou o par, não esconde mais

        isRevealed = false;
        iconImage.gameObject.SetActive(false); // Esconde o ícone (mostra apenas o fundo do botão)
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