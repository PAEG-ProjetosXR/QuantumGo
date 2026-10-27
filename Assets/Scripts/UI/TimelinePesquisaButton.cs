using UnityEngine;
using UnityEngine.UI;

public class TimelinePesquisaButton : MonoBehaviour
{
    PhysicistTimelineEraPesquisa pesquisaSo;
    public Color lockedColor;
    public GameObject infoPesquisaPanel;
    [SerializeField]
    private Button btnPesquisa;
    public void addData(PhysicistTimelineEraPesquisa pesquisaSo)
    {
        this.pesquisaSo = pesquisaSo;
        setupButtonPesquisa();
    }

    public void setupButtonPesquisa()
    {
        btnPesquisa.image.sprite = pesquisaSo.icone;
        Color color = btnPesquisa.image.color;
        color.a = 1f; // 1.0f is fully opaque (0.0f is completely transparent)
        if (!pesquisaSo.unlocked)
        {
            color = lockedColor;
        }
        btnPesquisa.image.color = color;
    }

    public void OnClick()
    {
        if (!pesquisaSo.unlocked)
        {
            return;
        }
        infoPesquisaPanel.GetComponent<InfoPesquisaPanel>().SetData(pesquisaSo);
        infoPesquisaPanel.SetActive(!infoPesquisaPanel.activeInHierarchy);
    }

}
