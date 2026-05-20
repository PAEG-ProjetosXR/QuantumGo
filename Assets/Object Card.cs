using UnityEngine;
using UnityEngine.UI;

public class ObjectCard : MonoBehaviour
{
    private ObjectData data;
    public Color32 unfoundColor = new Color32(180, 180, 180, 255);

    public void SetData(ObjectData newData)
    {
        data = newData;
        GetComponent<Image>().sprite = data.icon;
    }

    public void SetFound()
    {
        GetComponent<Image>().color = Color.white;
        data.foundTimes++;
    }
    public void SetFoundAgain()
    {
        data.foundTimes++;
    }
    public void SetUnfound()
    {
        GetComponent<Image>().color = unfoundColor;
        data.foundTimes = 0;
    }

    public void OnClick()
    {
        if (data != null && data.foundTimes > 0)
        {
            FindAnyObjectByType<UIHandler>().DisplayObjectDetails(data);
        }
    }
}
