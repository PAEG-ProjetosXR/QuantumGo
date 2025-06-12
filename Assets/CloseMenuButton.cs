using UnityEngine;

public class CloseMenuButton : MonoBehaviour
{
    private GameObject menuCanvas;
    private GameObject menuButton;
    private GameObject physpediaCanvas;
    private GameObject objpediaCanvas;

    private void Awake()
    {
        menuCanvas = GameObject.Find("MenuCanvas");
        menuButton = GameObject.Find("Menu Button");
        physpediaCanvas = GameObject.Find("Physpedia Canvas");
        objpediaCanvas = GameObject.Find("Objpedia Canvas");
    }
    
    public void onClick()
    {
        menuCanvas.SetActive(false);
        physpediaCanvas.SetActive(false);
        objpediaCanvas.SetActive(false);
        menuButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
