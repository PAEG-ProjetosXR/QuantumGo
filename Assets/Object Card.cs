using UnityEngine;
using UnityEngine.UI;

public class ObjectCard : MonoBehaviour
{
    private ObjectData data;
    public Color32 unfoundColor = new Color32(180, 180, 180, 255);

    private void OnEnable()
    {
        if (data && data.unlocked)
        {
            SetFound();
        }
    }

    public void SetData(ObjectData newData)
    {
        data = newData;
        GetComponent<Image>().sprite = data.icon;
    }

    public void SetFound()
    {
        GetComponent<Image>().color = Color.white;
    }
    public void SetUnfound()
    {
        GetComponent<Image>().color = unfoundColor;
        data.unlocked = false;
    }

    public void OnClick()
    {
        if (data != null && data.unlocked)
        {
            FindAnyObjectByType<UIHandler>().DisplayObjectDetails(data);
        }
    }
}
