using TMPro;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text infoText;
    
    public void onClick()
    {
        nameText.text = "";
        infoText.text = "";
    }
}
