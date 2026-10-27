using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPesquisaPanel : MonoBehaviour
{
    PhysicistTimelineEraPesquisa pesquisa;

    [SerializeField]
    private TextMeshProUGUI pesquisaNome;
    [SerializeField]
    private TextMeshProUGUI pesquisaAno;
    [SerializeField]
    private TextMeshProUGUI pesquisaDesc;
    [SerializeField]
    private Button fecharBtn;
    [SerializeField]
    private GameObject objetoPesquisaFirstBtn;
    [SerializeField]
    private ObjectData defaultObjectDataEmpty;

    public void SetData(PhysicistTimelineEraPesquisa pesquisa)
    {
        fecharBtn.onClick.AddListener(FecharMenu);
        this.pesquisa = pesquisa;
        setupUI();
    }

    private void setupUI()
    {
        pesquisaNome.text = pesquisa.titulo;
        pesquisaAno.text = pesquisa.ano.ToString();
        pesquisaDesc.text = pesquisa.desc;

        // Limpar objetos antigos de outra pesquisa que tinha sido aberta
        foreach(Transform child in objetoPesquisaFirstBtn.gameObject.transform.parent)
        {
            if (child.gameObject != objetoPesquisaFirstBtn)
            {
                Destroy(child.gameObject);
            }
        }

        if (!pesquisa.objetosPesquisa) {
            objetoPesquisaFirstBtn.GetComponent<InfoPesquisaObjetoBtn>().SetupObjetoPesquisaBtn(defaultObjectDataEmpty);
            return;
        }
        objetoPesquisaFirstBtn.GetComponent<InfoPesquisaObjetoBtn>().SetupObjetoPesquisaBtn(pesquisa.objetosPesquisa.correctObjects[0]);
        for (int i = 1; i < pesquisa.objetosPesquisa.correctObjects.Count; i++)
        {
            GameObject novoObjetoPesquisaBtn = Instantiate<GameObject>(objetoPesquisaFirstBtn, objetoPesquisaFirstBtn.transform.parent);
            novoObjetoPesquisaBtn.transform.SetAsLastSibling();
            novoObjetoPesquisaBtn.GetComponent<InfoPesquisaObjetoBtn>().SetupObjetoPesquisaBtn(pesquisa.objetosPesquisa.correctObjects[i]);
        }
    }

    private void FecharMenu()
    {
        this.gameObject.SetActive(false);
    }
}
