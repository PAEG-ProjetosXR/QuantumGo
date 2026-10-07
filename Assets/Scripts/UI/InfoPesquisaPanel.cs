using TMPro;
using UnityEngine;

public class InfoPesquisaPanel : MonoBehaviour
{
    PhysicistTimelineEraPesquisa pesquisa;

    [SerializeField]
    TextMeshProUGUI pesquisaNome;

    public void SetData(PhysicistTimelineEraPesquisa pesquisa)
    {
        this.pesquisa = pesquisa;
        setupUI();
    }

    private void setupUI()
    {
        pesquisaNome.text = pesquisa.titulo;
    }
}
