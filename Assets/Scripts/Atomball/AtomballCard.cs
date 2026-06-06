using UnityEngine;
using UnityEngine.UI;

public class AtomballCard : MonoBehaviour
{
    private static AtomballCard selectedCard;
    public AtomballDatabase database;

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

    public void SelectCard()
    {
        if (selectedCard != null)
            selectedCard.outline.enabled = false;

        selectedCard = this;
        outline.enabled = true;
        database.selectedBallId = id;

    }
}
