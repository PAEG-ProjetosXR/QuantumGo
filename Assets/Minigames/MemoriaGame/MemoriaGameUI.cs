using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class MemoriaGameUI : MinigameBase
{
    [Header("Configuração Visual")]
    [SerializeField] private GameObject cardButtonPrefab; // Prefab do botão da carta
    [SerializeField] private Transform cardsGridParent;   // O painel na UI com GridLayoutGroup

    private MemoriaGameData memoriaConfig;
    private int pairsFound = 0;
    private int totalTargetPairs = 0; // Quantos pares precisamos achar nesta partida

    // Variáveis de controle para a lógica de clique (Caso 3)
    private CardButton firstSelectedCard;
    private CardButton secondSelectedCard;
    private bool isChecking = false; // Trava para o jogador não clicar em 3 cartas ao mesmo tempo

    public override void Setup(MinigameData data)
    {
        memoriaConfig = (MemoriaGameData)data;

        int seed = UnityEngine.Random.Range(0, 999999);
        System.Random rng = new System.Random(seed);

        // Define quantos pares teremos com base na dificuldade ou dados
        totalTargetPairs = rng.Next(4, 5 + memoriaConfig.difficulty);

        pairsFound = 0;
        GenerateCards(rng, totalTargetPairs);
    }

    //Geração e Instanciação das Cartas
    private void GenerateCards(System.Random rng, int actualPairs)
    {
        // Estrutura temporária para guardar o ID e o Sprite juntos
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

        // Instancia os botões de fato na tela dentro do Grid
        foreach (var item in cardList)
        {
            GameObject cardObj = Instantiate(cardButtonPrefab, cardsGridParent);
            CardButton cardButtonComponent = cardObj.GetComponent<CardButton>();

            // Configura o botão passando seu ID único do par, o sprite e o gerenciador
            cardButtonComponent.SetupCard(item.id, item.sprite, this);
        }
    }

    //Lógica de Clique e Comparação de Pares
    public void OnCardClicked(CardButton clickedCard)
    {
        if (isChecking || clickedCard == firstSelectedCard) return;

        if (firstSelectedCard == null)
        {
            firstSelectedCard = clickedCard;
        }
        else
        {
            secondSelectedCard = clickedCard;
            isChecking = true;

            // Compara os IDs das duas cartas
            if (firstSelectedCard.GetCardID() == secondSelectedCard.GetCardID())
            {
                // ACERTOU O PAR!
                firstSelectedCard.SetMatched();
                secondSelectedCard.SetMatched();

                pairsFound++;
                ResetSelection();

                if (pairsFound >= totalTargetPairs)
                {
                    FinishMinigame(true); // Vitória!
                }
            }
            else
            {
                // ERROU! Chama a rotina para esperar um pouco e esconder as cartas de novo
                StartCoroutine(WrongPairRoutine());
            }
        }
    }

    // Rotina de espera caso o jogador erre o par
    private IEnumerator WrongPairRoutine()
    {
        yield return new WaitForSeconds(0.8f); // Tempo de espera na tela

        // Vira as cartas de volta para baixo
        firstSelectedCard.HideCard();
        secondSelectedCard.HideCard();

        ResetSelection();
    }

    private void ResetSelection()
    {
        firstSelectedCard = null;
        secondSelectedCard = null;
        isChecking = false;
    }

    private void OnTimeExpired()
    {
        FinishMinigame(false); // Derrota por tempo esgotado
    }

    // Estrutura auxiliar interna para organizar os dados antes de instanciar
    private struct CardPairData
    {
        public int id;
        public Sprite sprite;
        public CardPairData(int id, Sprite sprite) { this.id = id; this.sprite = sprite; }
    }
}
