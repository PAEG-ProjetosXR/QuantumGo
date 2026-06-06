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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(var atom in atomballDatabase.atomballs)
        {
            var newCard = Instantiate(atomballCardBtnPrefab);
            newCard.transform.GetComponentInChildren<TextMeshProUGUI>().text = atom.name; //temporário!
            newCard.GetComponent<UnityEngine.UI.Image>().sprite = atom.menuIcon;

            AtomballCard card = newCard.GetComponent<AtomballCard>();
            card.id = atom.id;

            Button button = newCard.GetComponent<Button>();
            button.onClick.AddListener(card.SelectCard);

            newCard.transform.SetParent(atomballViewContent.transform, false);
        }
    }

    private void OnButtonClick()
    {

    }
}
