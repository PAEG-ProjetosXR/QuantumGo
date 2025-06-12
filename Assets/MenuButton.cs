using UnityEngine;
using UnityEngine.UIElements;

public class MenuButton : MonoBehaviour
{
    private GameObject menuCanvas;
    private GameObject closeMenuButton;

    private void Awake()
    {
        menuCanvas = GameObject.Find("MenuCanvas");
        closeMenuButton = GameObject.Find("Close Menu Button");
    }
    
    public void onClick()
    {
        menuCanvas.SetActive(true);
        closeMenuButton.SetActive(true);
        gameObject.SetActive(false);
    }
}
