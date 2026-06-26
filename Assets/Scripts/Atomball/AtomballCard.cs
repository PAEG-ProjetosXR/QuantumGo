using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtomballCard : MonoBehaviour
{
    private static List<AtomballCard> allCards = new List<AtomballCard>();
    private static AtomballCard selectedCard;
    public static AtomballDatabase database;
    public TextMeshProUGUI atomballSelectInfoDescText;
    public TextMeshProUGUI atomballSelectInfoTitle;
    public GameObject atomballSelectInfo;
    public static Button selectAtomballBtn;

    public Outline outline;
    public int id;
    
    private void Start()
    {
        allCards.Add(this);
        outline.enabled = false;
        if (selectedCard == this)
        {
            outline.enabled = true;
        }
    }

    public static string generateAtomballText(Atomball atomball)
    {
        return $"\n\nDano: {atomball.damageToHealth} Vida{(atomball.damageToHealth > 1 ? "s" : "")}" +
               $"\nN° Paragráfos: {atomball.captureTimes}";

    }

    public static void disableOutlineAllCards()
    {
        foreach (var card in allCards)
        {
            card.outline.enabled = false;
        }
    }

    public static void disableOutlinesExceptForAtomballSelected()
    {
        foreach (var card in allCards)
        {
            if (card.id != database.selectedBallId)
            {
                card.outline.enabled = false;
            }
        }
    }

    public static void selectCardById(int id)
    {
        foreach (var card in allCards)
        {
            if (card.id == id)
            {
                card.SelectCard();
            }
        }
    }

    public void SelectCard()
    {
        if (selectedCard != null && selectedCard.id != database.selectedBallId)
            selectedCard.outline.enabled = false;

        outline.enabled = true;

        if (database.selectedBallId == id)
        {
            outline.effectColor = Color.red;
        }
        else
        {
            outline.effectColor = Color.black;
        }

        selectedCard = this;
        

        // UI info changes. This should happen AFTER the database update!
        atomballSelectInfo.SetActive(true);
        Atomball selectedAtomball = database.GetById(id);
        atomballSelectInfoTitle.text = selectedAtomball.name;
        atomballSelectInfoDescText.text = generateAtomballText(selectedAtomball);
        selectAtomballBtn.onClick.RemoveAllListeners();
        selectAtomballBtn.onClick.AddListener(selectAtomball);
    }

    public void selectAtomball()
    {
        disableOutlineAllCards();
        database.selectedBallId = id;
        SelectCard();
    }

    public void OnDestroy()
    {
        allCards.Remove(this);
    }

}
