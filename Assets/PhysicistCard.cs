using UnityEngine;
using UnityEngine.UI;

public class PhysicistCard : MonoBehaviour
{
    public Color32 unfoundColor = new Color32(180, 180, 180, 255);
    private PhysicistData data;

    public void SetData(PhysicistData newData)
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
            FindAnyObjectByType<UIHandler>().DisplayPhysicistDetails(data);
        }
    }
}
