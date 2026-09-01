using UnityEngine;
using UnityEngine.UI;

public class PhysicistCard : MonoBehaviour
{
    public Color32 unfoundColor = new Color32(180, 180, 180, 255);

    [SerializeField]
    private Image physicistImage;
    private PhysicistData data;

    public void SetData(PhysicistData newData)
    {
        data = newData;
        physicistImage.sprite = data.icon;
    }

    public void SetFound()
    {
        physicistImage.color = Color.white;
    }
    public void SetFoundAgain()
    {
    }
    public void SetUnfound()
    {
        physicistImage.color = unfoundColor;
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
