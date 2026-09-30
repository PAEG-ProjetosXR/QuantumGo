using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PhysicistTimeline : MonoBehaviour
{
    public Color32 unfoundColor = new Color32(180, 180, 180, 255);
    public Button physicistBtn;

    [SerializeField]
    private Image physicistImage;
    private PhysicistData data;
    [SerializeField]
    private GameObject timelineHeadPiece;
    [SerializeField]
    private GameObject timelineEraBody;
    [HideInInspector]
    public PhysicistTimelineData physicistTimelineSO;
    public GameObject timelineEnd;

    public void SetData(PhysicistData newData)
    {
        data = newData;
        physicistImage.sprite = data.icon;
        physicistTimelineSO = newData.physicistTimeline;
        SetupHead();
        SetupBody();
        timelineEnd.transform.SetAsLastSibling();
    }

    public void SetFound()
    {
        physicistImage.color = Color.white;
    }
    public void SetFoundAgain()
    {
    }
    public void SetUnfound()
    {
        physicistImage.color = unfoundColor;
        data.foundTimes = 0;
    }

    public void OnClick()
    {
        if (data != null && data.foundTimes > 0)
        {
            FindAnyObjectByType<UIHandler>().DisplayPhysicistDetails(data);
        }
    }

    private void setupButtonPesquisa(GameObject timelinePiece, Sprite image)
    {
        Button pesquisaBtn = timelinePiece.transform.GetComponentInChildren<Button>();
        pesquisaBtn.image.sprite = image;
        Color color = pesquisaBtn.image.color;
        color.a = 1f; // 1.0f is fully opaque (0.0f is completely transparent)
        pesquisaBtn.image.color = color;
    }
    public void SetupHead()
    {
        if (physicistTimelineSO == null)
        {
            return;
        }
        int qtdPieces = physicistTimelineSO.listaErasTimeline[0].listaPesquisas.Count;
        setupButtonPesquisa(timelineHeadPiece, physicistTimelineSO.listaErasTimeline[0].listaPesquisas[0].icone);
        for (int i = 1; i < qtdPieces; i++)
        {
            GameObject headPieceClone = Instantiate<GameObject>(timelineHeadPiece, timelineHeadPiece.transform.parent, false);
            setupButtonPesquisa(headPieceClone, physicistTimelineSO.listaErasTimeline[0].listaPesquisas[i].icone);
        }
    }
    public void SetupBody()
    {
        timelineEraBody.SetActive(false);
        if (physicistTimelineSO == null)
        {
            return;
        }
        for (int j = 1; j < physicistTimelineSO.listaErasTimeline.Count; j++)
        {

            GameObject eraBodyClone = Instantiate<GameObject>(timelineEraBody, timelineEraBody.transform.parent, false);
            eraBodyClone.SetActive(true);
            GameObject timelineBodyPiece = eraBodyClone.GetComponent<TimelineEraBody>().timelineBodyPiece;

            setupButtonPesquisa(timelineBodyPiece, physicistTimelineSO.listaErasTimeline[j].listaPesquisas[0].icone);
            int qtdPieces = physicistTimelineSO.listaErasTimeline[j].listaPesquisas.Count;
            
            for (int i = 1; i < qtdPieces; i++)
            {
                GameObject headPieceClone = Instantiate<GameObject>(timelineBodyPiece, timelineBodyPiece.transform.parent, false);
                setupButtonPesquisa(headPieceClone, physicistTimelineSO.listaErasTimeline[j].listaPesquisas[i].icone);
            }
        }
    }
}
