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
    }

    private void FecharMenu()
    {
        this.gameObject.SetActive(false);
    }
}
