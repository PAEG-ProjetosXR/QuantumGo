using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

//Vai ser anexado no Prefab da UI do Jogo da Mem�ria
public class MemoriaGameUI : MinigameBase
{
    [Header("Configura��o Visual")]
    [SerializeField] private GameObject cardButtonPrefab; // Prefab do bot�o da carta
    [SerializeField] private Transform cardsGridParent;   // O painel na UI com GridLayoutGroup


    [Header("Tela de Resultado")]
    [SerializeField] private GameObject resultPanel;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private Button closeButton;

    private bool gameFinished = false;
    private MemoriaGameData memoriaConfig;
    private int pairsFound;
    private int attempts;

    // Vari�veis de controle para a l�gica de clique (Caso 3)
    private CardButton firstSelectedCard;
    private CardButton secondSelectedCard;
    private CardButton pendingWrongFirst;
    private CardButton pendingWrongSecond;
    private Coroutine wrongPairCoroutine;
    private bool pendingWrongExhausted; // evitar vida extras clicando várias vezes
    private bool isChecking = false; // Trava para o jogador n�o clicar em 3 cartas ao mesmo tempo

    public override void Setup(MinigameData data)
    {
        memoriaConfig = (MemoriaGameData)data;

        int seed = UnityEngine.Random.Range(0, 999999);
        System.Random rng = new System.Random(seed);

        pairsFound = 0;
        attempts = memoriaConfig.maxAttempts;

        GenerateCards(rng, memoriaConfig.totalPairs);

        gameFinished = false;

        resultPanel.SetActive(false);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(
            MinigameManager.Instance.CloseResultPanel
        );
    }

    // mostrar tela de resultado
    public override void ShowResult(bool playerWon)
    {
        gameFinished = true;

        resultPanel.SetActive(true);

        if (playerWon)
        {
            resultText.text = "VITÓRIA!\nVocê encontrou todos os pares!";
        }
        else
        {
            resultText.text = "DERROTA!\nSuas tentativas acabaram.";
        }
    }

    //Gera��o e Instancia��o das Cartas
    private void GenerateCards(System.Random rng, int actualPairs)
    {
        // Estrutura tempor�ria para guardar o ID e o Sprite juntos
        List<CardPairData> cardList = new List<CardPairData>();

        // Pega os pares configurados no ScriptableObject
        for (int i = 0; i < actualPairs; i++)
        {
            Sprite spriteCard = memoriaConfig.cardSprites[i];

            // Criamos duas cartas com o MESMO ID para formar o par
            cardList.Add(new CardPairData(i, spriteCard));
            cardList.Add(new CardPairData(i, spriteCard));
        }

        // Embaralha a lista usando o algoritmo Fisher-Yates com a Seed (rng)
        int n = cardList.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardPairData temp = cardList[k];
            cardList[k] = cardList[n];
            cardList[n] = temp;
        }

        // Instancia os bot�es de fato na tela dentro do Grid
        foreach (var item in cardList)
        {
            GameObject cardObj = Instantiate(cardButtonPrefab, cardsGridParent);
            CardButton cardButtonComponent = cardObj.GetComponent<CardButton>();

            // Configura o bot�o passando seu ID �nico do par, o sprite e o gerenciador
            cardButtonComponent.SetupCard(item.id, item.sprite, this);
        }
    }

    //L�gica de Clique e Compara��o de Pares
    public void OnCardClicked(CardButton clickedCard)
    {
        if (gameFinished)
            return;

        // Se havia um par errado esperando para ser escondido,
        // esconde-o imediatamente ao receber um novo clique.
        if (pendingWrongFirst != null)
        {
            // Se era a última tentativa, não permite continuar
            if (pendingWrongExhausted)
                return;

            StopCoroutine(wrongPairCoroutine);

            pendingWrongFirst.HideCard();
            pendingWrongSecond.HideCard();

            pendingWrongFirst = null;
            pendingWrongSecond = null;
            pendingWrongExhausted = false;
            wrongPairCoroutine = null;

            ResetSelection();
        }

        if (isChecking || clickedCard == firstSelectedCard)
            return;

        clickedCard.RevealCard();

        if (firstSelectedCard == null)
        {
            firstSelectedCard = clickedCard;
            return;
        }

        secondSelectedCard = clickedCard;
        isChecking = true;

        attempts--;

        if (firstSelectedCard.GetCardID() == secondSelectedCard.GetCardID())
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();

            pairsFound++;

            ResetSelection();

            if (pairsFound >= memoriaConfig.totalPairs)
            {
                FinishMinigame(true);
                return;
            }

            if (attempts <= 0)
            {
                OnAttemptsExhausted();
                return;
            }
        }
        else
        {
            // Guarda explicitamente as cartas erradas
            pendingWrongFirst = firstSelectedCard;
            pendingWrongSecond = secondSelectedCard;
            pendingWrongExhausted = attempts <= 0;

            wrongPairCoroutine = StartCoroutine(
                WrongPairRoutine(attempts <= 0)
            );
        }
    }

    // Rotina de espera caso o jogador erre o par
    private IEnumerator WrongPairRoutine(bool exhaustedAttempts)
    {
        yield return new WaitForSeconds(0.8f);

        // Verifica se ainda existe um par pendente
        if (pendingWrongFirst != null)
        {
            pendingWrongFirst.HideCard();
            pendingWrongSecond.HideCard();

            pendingWrongFirst = null;
            pendingWrongSecond = null;
            wrongPairCoroutine = null;
            pendingWrongExhausted = false;

            ResetSelection();
        }

        if (exhaustedAttempts)
        {
            OnAttemptsExhausted();
        }
    }

    private void ResetSelection()
    {
        firstSelectedCard = null;
        secondSelectedCard = null;
        isChecking = false;
    }

    //Derrota pelo esgotamento de tentativas
    private void OnAttemptsExhausted() { FinishMinigame(false); }

    //Derrota por tempo esgotado
    private void OnTimeExpired() { FinishMinigame(false); }

    // Estrutura auxiliar interna para organizar os dados antes de instanciar
    private struct CardPairData
    {
        public int id;
        public Sprite sprite;
        public CardPairData(int id, Sprite sprite) { this.id = id; this.sprite = sprite; }
    }
}
