using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AtomballCard : MonoBehaviour
{
    private static AtomballCard selectedCard;
    public AtomballDatabase database;
    public TextMeshProUGUI atomballSelectInfoDescText;
    public TextMeshProUGUI atomballSelectInfoTitle;
    public GameObject atomballSelectInfo;

    public Outline outline;
    public int id;
    
    private void Start()
    {
        outline.enabled = false;
        if (selectedCard == this)
        {
            outline.enabled = true;
        }
    }

    public static string generateAtomballText(Atomball atomball)
    {
        return $"Descrição curta da atomball" +
            $"\n\nDano: {atomball.damageToHealth} Vida{(atomball.damageToHealth > 1 ? "s" : "")}" +
            $"\nN° Paragráfos: {atomball.captureTimes}";
    }

    public void SelectCard()
    {
        if (selectedCard != null)
            selectedCard.outline.enabled = false;

        selectedCard = this;
        outline.enabled = true;
        database.selectedBallId = id;

        // UI info changes. This should happen AFTER the database update!
        atomballSelectInfo.SetActive(true);
        Atomball selectedAtomball = database.GetChosenAtomball();
        atomballSelectInfoTitle.text = selectedAtomball.name;
        atomballSelectInfoDescText.text = generateAtomballText(selectedAtomball);

    }
}
