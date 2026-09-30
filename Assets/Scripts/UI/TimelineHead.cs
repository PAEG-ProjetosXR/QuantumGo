using UnityEngine;
using UnityEngine.UI;

public class TimelineHead : MonoBehaviour
{
    public GameObject dividerStart;
    public GameObject timelineHeadPiece;
    private PhysicistTimelineData physicistTimelineSO;

    public void addData(PhysicistTimelineData physicistTimelineSO)
    {
        this.physicistTimelineSO = physicistTimelineSO;
    }

    public static void setDividerColor(GameObject divider, Color color)
    {
        divider.GetComponent<Image>().color = color;
    }

    public void SetupHead()
    {
        if (physicistTimelineSO == null)
        {
            return;
        }
        int qtdPieces = physicistTimelineSO.listaErasTimeline[0].listaPesquisas.Count;
        timelineHeadPiece.GetComponentInChildren<TimelinePesquisaButton>().addData(physicistTimelineSO.listaErasTimeline[0].listaPesquisas[0]);
        setDividerColor(dividerStart, physicistTimelineSO.listaErasTimeline[0].corDivisoria);
        for (int i = 1; i < qtdPieces; i++)
        {
            GameObject headPieceClone = Instantiate<GameObject>(timelineHeadPiece, timelineHeadPiece.transform.parent, false);
            headPieceClone.GetComponentInChildren<TimelinePesquisaButton>().addData(physicistTimelineSO.listaErasTimeline[0].listaPesquisas[i]);
        }
    }
}
