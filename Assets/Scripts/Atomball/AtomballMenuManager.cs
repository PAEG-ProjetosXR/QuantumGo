using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AtomballMenuManager : MonoBehaviour
{
    public AtomballDatabase atomballDatabase;
    public GameObject atomballMenu;
    public GameObject atomballViewContent;
    public GameObject atomballCardBtnPrefab;
    public TextMeshProUGUI atomballSelectInfoDescText;
    public TextMeshProUGUI atomballSelectInfoTitle;
    public GameObject atomballSelectInfo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bool firstTime = true;
        foreach(var atom in atomballDatabase.atomballs)
        {
            var newCard = Instantiate(atomballCardBtnPrefab);
            newCard.transform.GetComponentInChildren<TextMeshProUGUI>().text = atom.name; //temporário!
            newCard.GetComponent<UnityEngine.UI.Image>().sprite = atom.menuIcon;

            AtomballCard card = newCard.GetComponent<AtomballCard>();
            card.id = atom.id;
            card.database = atomballDatabase;
            card.atomballSelectInfo = atomballSelectInfo;
            card.atomballSelectInfoDescText = atomballSelectInfoDescText;
            card.atomballSelectInfoTitle = atomballSelectInfoTitle;

            Button button = newCard.GetComponent<Button>();
            button.onClick.AddListener(card.SelectCard);

            if (firstTime)
            {
                card.SelectCard();
                firstTime = false;
            }

            newCard.transform.SetParent(atomballViewContent.transform, false);
        }
    }

    private void OnButtonClick()
    {

    }
}
