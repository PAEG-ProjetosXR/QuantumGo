using PokemonGO.Code;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject detailPanel;
    public GameObject menuPanel;
    public GameObject physpediaPanel;
    public GameObject objpediaPanel;
    public GameObject physpediaButton;
    public GameObject objpediaButton;
    public Button captureButton;
    public GameObject atomballButton;
    public GameObject atomballMenu;
    public GridLayoutGroup atomballGridLayout;
    public float atomballMenuMaxWidth;
    private TouchTest touchTest;
    [SerializeField] private Image detailImage;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text detailBio;
    [SerializeField] private Image spawnButtonImage;
    public TMP_Text cooldownLabelPrefab;
    public RectTransform cooldownContainer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        getComponents();
    }

    private void Start()
    {
        hideUI();
        disableCapture();
    }

    private void getComponents()
    {
        touchTest = GameObject.Find("XR Origin").GetComponent<TouchTest>();
        if (Thrower.Instance == null)
        {
            Debug.LogError("sem Thrower");
        }
    } // or more fields
    
    public void DisplayPhysicistDetails(PhysicistData data)
    {
        hideUIWithExcept(detailPanel);
        detailPanel.SetActive(true);
        detailImage.sprite = data.icon;
        
        if (data.foundTimes > 0)
        {
            string[] descriptionParts = data.description.Split('\n');
            detailImage.color = Color.white;
            detailName.text = "Nome: " + data.name;
            detailBio.text = "Descrição: \n" + string.Join("\n",descriptionParts.Take(data.foundTimes));
            int timesLeft = descriptionParts.Length - data.foundTimes;
            string timesText = timesLeft == 1 ? " vez" : " vezes";
            if (timesLeft > 0)
            {
                detailBio.text += "\n[Capturar mais " + timesLeft + timesText + " para desbloquear a descrição completa!]";
            }
        }
        else
        {
            detailImage.color = new Color32(0x07, 0x07, 0x07, 255);
            detailName.text = "Nome: ???" ;
            detailBio.text = "?????";
        }
    }
    
    public void DisplayObjectDetails(ObjectData data)
    {
        hideUIWithExcept(detailPanel);
        detailPanel.SetActive(true);
        detailImage.sprite = data.icon;
        detailName.text = "Nome: " + data.name;
        detailBio.text = "Descrição: \n" + data.description;
    }
    
    private void hideUIWithExcept(GameObject exceptThis)
    {
        var state = exceptThis.activeInHierarchy;

        physpediaPanel.SetActive(false);
        objpediaPanel.SetActive(false);
        detailPanel.SetActive(false);
        atomballMenu.SetActive(false);

        exceptThis.SetActive(state);
    }

    private void hideUI()
    {
        physpediaPanel.SetActive(false);
        objpediaPanel.SetActive(false);
        detailPanel.SetActive(false);
        atomballMenu.SetActive(false);
    }

    public void enableCapture()
    {
        captureButton.interactable = true;
    }

    public void disableCapture()
    {
        captureButton.interactable = false;
    }

    public void pokeballButton()
    {
        hideUI();
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

    public void openPhyspedia()
    {
        hideUIWithExcept(physpediaPanel);
        physpediaPanel.SetActive(!physpediaPanel.activeInHierarchy);
        //touchTest.canInteract = false;
    }

    public void openObjpedia()
    {
        hideUIWithExcept(objpediaPanel);
        objpediaPanel.SetActive(!objpediaPanel.activeInHierarchy);
        //touchTest.canInteract = false;
    }

    public static void UpdateCellSizeAtomball(
    GridLayoutGroup grid,
    int columns)

    {
        RectTransform rect = grid.GetComponent<RectTransform>();

        float totalWidth = rect.rect.width;
        float spacing = grid.spacing.x;
        float padding = grid.padding.left + grid.padding.right;

        float cellWidth =
            (totalWidth - padding - spacing * (columns - 1))
            / columns;

        grid.cellSize = new Vector2(cellWidth, cellWidth);
    }

    public void openAtomballMenu()
    {
        hideUIWithExcept(atomballMenu);
        atomballMenu.SetActive(!atomballMenu.activeInHierarchy);
        if (atomballMenu.activeInHierarchy)
        {
            Canvas.ForceUpdateCanvases();

            RectTransform rect = atomballMenu.GetComponent<RectTransform>();

            float width = Mathf.Min(
                ((RectTransform)rect.parent).rect.width,
                atomballMenuMaxWidth);

            rect.SetSizeWithCurrentAnchors(
                RectTransform.Axis.Horizontal,
                width);

            UpdateCellSizeAtomball(atomballGridLayout, 2);
        }
    }

}

