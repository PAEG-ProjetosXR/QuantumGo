using PokemonGO.Code;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
    public GameObject detailPanel;
    public GameObject minigamePanel;
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
    public GameObject atomballSelectInfo;
    public GameObject settingsMenuPanel;
    public GameObject infoPesquisaPanel;
    public GameObject objectOnTouchPanel;
    public Button minigameButton;
    public Button closeObjButton;
    private TouchTest touchTest;
    [SerializeField] private MinigameManager minigameManager;
    [SerializeField] private MemoriaGameData memoriaGameData; 
    [SerializeField] private GameObject arInterface;
    [SerializeField] private Image detailImage;
    [SerializeField] private TMP_Text detailName;
    [SerializeField] private TMP_Text detailBio;
    [SerializeField] private Image spawnButtonImage;
    [SerializeField] private TMP_Text objNameText;
    [SerializeField] private TMP_Text objDescText;
    [SerializeField] private TMP_Text logicListText;
    [SerializeField] private SpawnManager spawnManager;
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

    private void hideUIWithExcept(GameObject exceptThis)
    {
        var state = exceptThis.activeInHierarchy;

        hideUI();

        exceptThis.SetActive(state);
    }

    private void hideUI()
    {
        physpediaPanel.SetActive(false);
        objpediaPanel.SetActive(false);
        detailPanel.SetActive(false);
        minigamePanel.SetActive(false);
        atomballMenu.SetActive(false);
        atomballSelectInfo.SetActive(false);
        settingsMenuPanel.SetActive(false);
        infoPesquisaPanel.SetActive(false);
        objectOnTouchPanel.SetActive(false);
        closeObjButton.onClick.AddListener(closeObjectUi);
    }

    public void HideUIForMinigame()
    {
        arInterface.SetActive(false);
    }

    public void ShowUIAfterMinigame()
    {
        arInterface.SetActive(true);
    }

    #region Objepedia e Physipedia

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

    #endregion

    #region Capture

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

    public void generateObjectUi(ObjectData data)
    {
        hideUI();
        if (data != null)
        {
            objectOnTouchPanel.SetActive(true);
            objDescText.text = data.description;
            objNameText.text = data.name;
        }
            else
        {
            Debug.LogError("Object Trigger nao existe");
            spawnButtonImage.color = Color.red;
        }

    }

    public void closeObjectUi()
    {
        hideUI();
    }


    #endregion

    #region Atomball Menu
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

    public void IniciarJogoDaMemoria()
    {
    minigamePanel.SetActive(true);
    minigameManager.StartMinigame(memoriaGameData);
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

            SpawnPattern currentPattern = spawnManager.GetCurrentSpawnPattern();
            List<string> currentLogicList = spawnManager.GetCurrentLogicList(); 

            logicListText.text = "";
            if(currentPattern != null)
            {
                spawnManager.initializeCurrentLogicList(); // numero de entradas na lista = esperado pelo spawn pattern
                for (int i = 0; i < currentLogicList.Count; i++)
                {
                    logicListText.text = logicListText.text + currentPattern.logicList[i] + " ";
                    logicListText.text = logicListText.text + currentLogicList[i] + ". ";

                }
            } else
            {
                logicListText.text = "Inicie uma captura de físico para ve a lista lógica!";
            }
            
        }
    }

    #endregion

    #region Settings Menu
    public void openSettingsMenu()
    {
        hideUI();
        settingsMenuPanel.SetActive(true);

    }

    #endregion

}

