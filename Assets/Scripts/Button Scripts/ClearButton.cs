using TMPro;
using UnityEngine;

public class ClearButton : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text infoText;
    
    public void ClickButton()
    {
        nameText.text = "";
        infoText.text = "";
    }
}
