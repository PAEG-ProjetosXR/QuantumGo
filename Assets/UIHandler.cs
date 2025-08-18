using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    private GameObject physicistCanvas;
    private GameObject objectCanvas;
    private GameObject detailCanvas;
    private GameObject menuCanvas;
    private GameObject closeMenuButton;
    private GameObject goBackButton;
    private GameObject menuButton;
    private GameObject physpediaCanvas;
    private GameObject objpediaCanvas;
    private GameObject physpediaButton;
    private GameObject objpediaButton;
    private TouchTest touchTest;
    [SerializeField] private Image detailImage;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text detailBio;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        getComponents();
        hideUI();
    }

    private void getComponents()
    {
        physicistCanvas = GameObject.Find("Physicist Canvas");
        objectCanvas = GameObject.Find("Object Canvas");
        closeMenuButton = GameObject.Find("Close Menu Button");
        menuCanvas = GameObject.Find("MenuCanvas");
        goBackButton = GameObject.Find("Go Back Button");
        physpediaCanvas = GameObject.Find("Physpedia Canvas");
        objpediaCanvas = GameObject.Find("Objpedia Canvas");
        physpediaButton = GameObject.Find("Physpedia Button");
        objpediaButton = GameObject.Find("Objpedia Button");
        menuButton = GameObject.Find("Menu Button");
        detailCanvas = GameObject.Find("Detail Canvas");
        touchTest = GameObject.Find("XR Origin").GetComponent<TouchTest>();
    } // or more fields
    
    public void DisplayPhysicistDetails(PhysicistData data)
    {
        detailCanvas.SetActive(true);
        detailImage.sprite = data.icon;
        if (data.quest.questCompleted)
        {
            detailImage.color = Color.white;
            detailName.text = "Nome: " + data.name;
            detailBio.text = "Descrição: \n" + data.description;
        }
        else
        {
            detailImage.color = new Color32(0x07, 0x07, 0x07, 255);
            detailName.text = "Nome: ???" ;
            detailBio.text = "?????";
        }
        
        physpediaCanvas.SetActive(false);
        physpediaButton.SetActive(true);
        objpediaButton.SetActive(true);
    }
    
    public void DisplayObjectDetails(ObjectData data)
    {
        detailCanvas.SetActive(true);
        detailImage.sprite = data.icon;
        detailName.text = "Nome: " + data.name;
        detailBio.text = "Descrição: \n" + data.description;
        objpediaCanvas.SetActive(false);
        physpediaButton.SetActive(true);
        objpediaButton.SetActive(true);
    }
    
    private void hideUI()
    {
        physicistCanvas.SetActive(false);
        objectCanvas.SetActive(false);
        physpediaCanvas.SetActive(false);
        objpediaCanvas.SetActive(false);
        detailCanvas.SetActive(false);
    }

    public void returnToGame()
    {
        physpediaButton.SetActive(true);
        objpediaButton.SetActive(true);
        objpediaCanvas.SetActive(false);
        physpediaCanvas.SetActive(false);
        detailCanvas.SetActive(false);
        touchTest.canInteract = true;
    }

    public void closeMenu()
    {
        physpediaCanvas.SetActive(false);
        objpediaCanvas.SetActive(false);
        physicistCanvas.SetActive(false);
        objectCanvas.SetActive(false);
        
    }

    public void goBack()
    {
        Transform current = transform;
        GameObject parentCanvas = null;
        GameObject grandparentCanvas = null;

        // Step 1: Find parent canvas
        while (current.parent != null)
        {
            current = current.parent;
            if (current.GetComponent<Canvas>() != null)
            {
                parentCanvas = current.gameObject;
                break;
            }
        }

        if (parentCanvas == null)
        {
            Debug.LogWarning("Parent canvas not found.");
            return;
        }

        // Step 2: From parent, look for grandparent canvas
        current = current.parent;
        while (current != null)
        {
            if (current.GetComponent<Canvas>() != null)
            {
                grandparentCanvas = current.gameObject;
                break;
            }
            current = current.parent;
        }

        if (grandparentCanvas == null)
        {
            Debug.LogWarning("Grandparent canvas not found.");
            return;
        }

        // Step 3: Toggle canvases
        parentCanvas.SetActive(false);
        grandparentCanvas.SetActive(true);
    
    }

    public void openPhyspedia()
    {
        physpediaButton.SetActive(false);
        objpediaButton.SetActive(false);
        physpediaCanvas.SetActive(true);
        detailCanvas.SetActive(false);
        touchTest.canInteract = false;
    }

    public void openObjpedia()
    {
        physpediaButton.SetActive(false);
        objpediaButton.SetActive(false);
        objpediaCanvas.SetActive(true);
        detailCanvas.SetActive(false);
        touchTest.canInteract = false;
    }

}

