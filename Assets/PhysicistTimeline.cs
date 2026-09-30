using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhysicistTimeline : MonoBehaviour
{
    public Color32 unfoundColor = new Color32(180, 180, 180, 255);
    public Button physicistBtn;

    [SerializeField]
    private Image physicistImage;
    private PhysicistData data;
    public TimelineHead headScript;
    public TimelineEraBody bodyScript;
    public GameObject timelineEnd;

    public void SetData(PhysicistData newData)
    {
        data = newData;
        physicistImage.sprite = data.icon;

        headScript.addData(newData.physicistTimeline);
        GameObject bodyPiece = Instantiate<GameObject>(headScript.timelineHeadPiece, bodyScript.timeline.transform, false);
        bodyPiece.SetActive(false);
        bodyScript.addData(newData.physicistTimeline, bodyPiece);

        headScript.SetupHead();
        bodyScript.SetupBody();
        timelineEnd.transform.SetAsLastSibling();
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
