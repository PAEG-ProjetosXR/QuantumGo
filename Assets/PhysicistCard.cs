using UnityEngine;
using UnityEngine.UI;

public class PhysicistCard : MonoBehaviour
{
    
    private PhysicistData data;

    public void SetData(PhysicistData newData)
    {
        data = newData;
        GetComponent<Image>().sprite = data.icon;
    }

    public void OnClick()
    {
        if (data != null)
        {
            FindObjectOfType<UIHandler>().DisplayPhysicistDetails(data);
        }
    }
}
