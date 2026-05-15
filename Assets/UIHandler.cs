using PokemonGO.Code;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject physicistPanel;
    public GameObject objectPanel;
    public GameObject detailPanel;
    public GameObject menuPanel;
    public GameObject physpediaPanel;
    public GameObject objpediaPanel;
    public GameObject physpediaButton;
    public GameObject objpediaButton;
    public GameObject captureButton;
    private TouchTest touchTest;
    [SerializeField] private Image detailImage;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text detailBio;
    [SerializeField] private Image spawnButtonImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        getComponents();
    }

    private void Start()
    {
        hideUI();
    }

    private void getComponents()
    {

        //objpediaButton = GameObject.Find("Capture Button"); ????????????????

        touchTest = GameObject.Find("XR Origin").GetComponent<TouchTest>();
        if (Thrower.Instance == null)
        {
            Debug.LogError("sem Thrower");
        }
    } // or more fields
    
    public void DisplayPhysicistDetails(PhysicistData data)
    {
        detailPanel.SetActive(true);
        detailImage.sprite = data.icon;
        
        if (data.foundTimes > 0)
        {
            string[] descriptionParts = data.description.Split('\n');
            detailImage.color = Color.white;
            detailName.text = "Nome: " + data.name;
            detailBio.text = "Descrição: \n" + string.Join("\n",descriptionParts.Take(data.foundTimes));
        }
        else
        {
            detailImage.color = new Color32(0x07, 0x07, 0x07, 255);
            detailName.text = "Nome: ???" ;
            detailBio.text = "?????";
        }
        
        physpediaPanel.SetActive(false);
        physpediaButton.SetActive(true);
        objpediaButton.SetActive(true);
    }
    
    public void DisplayObjectDetails(ObjectData data)
    {
        detailPanel.SetActive(true);
        detailImage.sprite = data.icon;
        detailName.text = "Nome: " + data.name;
        detailBio.text = "Descrição: \n" + data.description;
        objpediaPanel.SetActive(false);
        physpediaButton.SetActive(true);
        objpediaButton.SetActive(true);
        captureButton.SetActive(true);
    }
    
    private void hideUI()
    {
        physicistPanel.SetActive(false);
        objectPanel.SetActive(false);
        physpediaPanel.SetActive(false);
        objpediaPanel.SetActive(false);
        detailPanel.SetActive(false);
    }

    public void returnToGame()
    {
        physpediaButton?.SetActive(true);
        objpediaButton?.SetActive(true);
        objpediaPanel?.SetActive(false);
        physpediaPanel?.SetActive(false);
        detailPanel?.SetActive(false);
        captureButton?.SetActive(true);
        //pokeSpawn.SpawnPokeBall();
        touchTest.canInteract = true;
    }

    public void pokeballButton()
    {
        if (Thrower.Instance != null)
        {
            Thrower.Instance.SpawnPokeBall();
        }
            else
        {
            Debug.LogError("Thrower nao existe");
            spawnButtonImage.color = Color.red;
        }

    }

    public void closeMenu()
    {
        physpediaPanel.SetActive(false);
        objpediaPanel.SetActive(false);
        physicistPanel.SetActive(false);
        objectPanel.SetActive(false);
        captureButton.SetActive(true);
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
        physpediaButton?.SetActive(false);
        objpediaButton?.SetActive(false);
        physpediaPanel?.SetActive(true);
        detailPanel?.SetActive(false);
        captureButton?.SetActive(false);
        touchTest.canInteract = false;
    }

    public void openObjpedia()
    {
        physpediaButton?.SetActive(false);
        objpediaButton?.SetActive(false);
        objpediaPanel?.SetActive(true);
        detailPanel?.SetActive(false);
        captureButton?.SetActive(false);
        touchTest.canInteract = false;
    }

}

