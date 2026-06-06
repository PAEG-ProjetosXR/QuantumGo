using UnityEngine;
using UnityEngine.UI;

public class AtomballCard : MonoBehaviour
{
    private static AtomballCard selectedCard;
    public static AtomballDatabase database;

    public Outline outline;
    public int id;
    
    private void Awake()
    {
        outline.enabled = false;
    }

    public void SelectCard()
    {
        if (selectedCard != null)
            selectedCard.outline.enabled = false;

        selectedCard = this;
        outline.enabled = true;
        database.selectedBallId = id;

    }
}
