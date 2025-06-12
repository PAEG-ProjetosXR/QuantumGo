using UnityEngine;

public class OpenObjpedia : MonoBehaviour
{
    private GameObject physpediaButton;
    private GameObject objpediaButton;
    private GameObject objpediaCanvas;

    private void Awake()
    {
        physpediaButton = GameObject.Find("Physpedia Button");
        objpediaButton = GameObject.Find("Objpedia Button");
        objpediaCanvas = GameObject.Find("ObjpediaCanvas");
    }

    public void onClick()
    {
        physpediaButton.SetActive(false);
        objpediaButton.SetActive(false);
        objpediaCanvas.SetActive(true);
    }
}
