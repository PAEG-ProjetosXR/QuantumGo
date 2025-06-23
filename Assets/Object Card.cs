using UnityEngine;
using UnityEngine.UI;

public class ObjectCard : MonoBehaviour
{
    private ObjectData data;

    public void SetData(ObjectData newData)
    {
        data = newData;
        GetComponent<Image>().sprite = data.icon;
    }

    public void OnClick()
    {
        if (data != null)
        {
            FindObjectOfType<UIHandler>().DisplayObjectDetails(data);
        }
    }
}
