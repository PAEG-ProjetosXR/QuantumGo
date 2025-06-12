using UnityEngine;

public class UIHandler : MonoBehaviour
{
    private GameObject physicistCanvas;
    private GameObject objectCanvas;
    private GameObject menuCanvas;
    private GameObject closeMenuButton;
    private GameObject goBackButton;
    private GameObject menuButton;
    private GameObject physpediaCanvas;
    private GameObject objpediaCanvas;
    private GameObject physpediaButton;
    private GameObject objpediaButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        getComponents();
        hideUI();
    }

    private void getComponents()
    {
        physicistCanvas = GameObject.Find("PhysicistCanvas");
        objectCanvas = GameObject.Find("ObjectCanvas");
        closeMenuButton = GameObject.Find("Close Menu Button");
        menuCanvas = GameObject.Find("MenuCanvas");
        goBackButton = GameObject.Find("Go Back Button");
        physpediaCanvas = GameObject.Find("PhyspediaCanvas");
        objpediaCanvas = GameObject.Find("ObjpediaCanvas");
        physpediaButton = GameObject.Find("Physpedia Button");
        objpediaButton = GameObject.Find("Objpedia Button");
        menuButton = GameObject.Find("Menu Button");
    }
    
    private void hideUI()
    {
        menuCanvas.SetActive(false);
    }

    public void openMenu()
    {
        menuCanvas.SetActive(true);
        closeMenuButton.SetActive(true);
        menuButton.SetActive(false);
        physpediaButton.SetActive(true);
        objpediaButton.SetActive(true);
        objpediaCanvas.SetActive(false);
        physpediaCanvas.SetActive(false);
    }

    public void closeMenu()
    {
        menuCanvas.SetActive(false);
        menuButton.SetActive(true);
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
    }

    public void openObjpedia()
    {
        physpediaButton.SetActive(false);
        objpediaButton.SetActive(false);
        objpediaCanvas.SetActive(true);
    }

}

