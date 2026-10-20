using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoPesquisaObjetoBtn : MonoBehaviour
{
    [DoNotSerialize]
    public ObjectData objetoPesquisa;

    public void SetupObjetoPesquisaBtn(ObjectData objetoPesquisa)
    {
        this.objetoPesquisa = objetoPesquisa;
        this.gameObject.transform.GetComponent<Image>().sprite = this.objetoPesquisa.icon;
        
    }

    public void OnClick()
    {
        FindAnyObjectByType<UIHandler>().DisplayObjectDetails(objetoPesquisa);
    }
}
