using UnityEngine;
using UnityEngine.UI;

public class TimelineEraBody : MonoBehaviour
{
    public GameObject dividerStart;
    private GameObject timelineBodyPiece;
    public GameObject timeline;
    public GameObject timelineEraBody;

    [HideInInspector]
    public PhysicistTimelineData physicistTimelineSO;

    public void addData(PhysicistTimelineData physicistTimelineSO, GameObject timelineBodyPiece)
    {
        this.physicistTimelineSO = physicistTimelineSO;
        this.timelineBodyPiece = timelineBodyPiece;
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
            TimelineEraBody script = eraBodyClone.GetComponent<TimelineEraBody>();
            script.timelineBodyPiece = Instantiate<GameObject>(this.timelineBodyPiece, timeline.transform, false);
            GameObject timelineBodyPiece = script.timelineBodyPiece;
            TimelineHead.setDividerColor(script.dividerStart, physicistTimelineSO.listaErasTimeline[j].corDivisoria);

            timelineBodyPiece.GetComponentInChildren<TimelinePesquisaButton>().addData(physicistTimelineSO.listaErasTimeline[j].listaPesquisas[0]);
            int qtdPieces = physicistTimelineSO.listaErasTimeline[j].listaPesquisas.Count;

            for (int i = 1; i < qtdPieces; i++)
            {
                GameObject headPieceClone = Instantiate<GameObject>(timelineBodyPiece, timelineBodyPiece.transform.parent, false);
                headPieceClone.GetComponentInChildren<TimelinePesquisaButton>().addData(physicistTimelineSO.listaErasTimeline[j].listaPesquisas[i]);
            }
        }
    }
}
