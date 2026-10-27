using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InfoPesquisaObjetoBtn : MonoBehaviour
{
    public Color lockedColor;
    [DoNotSerialize]
    public ObjectData objetoPesquisa;

    public void SetupObjetoPesquisaBtn(ObjectData objetoPesquisa)
    {
        this.objetoPesquisa = objetoPesquisa;
        var imagem = this.gameObject.transform.GetComponent<Image>();
        imagem.sprite = this.objetoPesquisa.icon;
        if (!objetoPesquisa.unlocked)
        {
            imagem.color = lockedColor;
        }
    }

    public void OnClick()
    {
        if (!objetoPesquisa.unlocked)
        {
            return;
        }
        FindAnyObjectByType<UIHandler>().DisplayObjectDetails(objetoPesquisa);
    }
}
