using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TouchTest : MonoBehaviour
{
    private EncounterManager encounterManager;
    public TMP_Text popupText;

    public TMP_Text nameText;
    public TMP_Text infoText;
    
    public static Canvas canvas;
    
    public Canvas PhysicistCanvas;
    public Canvas ObjectCanvas;
    
    public GameObject namePanel;
    public GameObject infoPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        encounterManager = FindObjectOfType<EncounterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed primary button");
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Debug.Log("hit");
                
                if (hit.transform.CompareTag("Physicist"))
                {
                    PhysicistCanvas.gameObject.SetActive(true);
                    Debug.Log(hit.transform.name + " : " + hit.transform.tag);
                    PhysicistInteraction(hit);
                    
                }

                if (hit.transform.CompareTag("Object"))
                {
                    ObjectCanvas.gameObject.SetActive(true);
                    Debug.Log(hit.transform.name + " : " + hit.transform.tag);
                    ObjectInteraction(hit);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (PhysicistCanvas.gameObject.activeInHierarchy)
            {
                PhysicistCanvas.gameObject.SetActive(false);
            }

            if (ObjectCanvas.gameObject.activeInHierarchy)
            {
                ObjectCanvas.gameObject.SetActive(false);
            }
        }
    }

    private void PhysicistInteraction(RaycastHit hit)
    {
        PhysicistTrigger physicistTrigger = hit.transform.GetComponent<PhysicistTrigger>();
        PhysicistData physicistData = physicistTrigger.data;
        namePanel.GetComponent<Image>().color = physicistData.dialogue.lines[0].Color;
        infoPanel.GetComponent<Image>().color = physicistData.dialogue.lines[0].Color;
        if (physicistTrigger != null)
        {
            physicistTrigger.TriggerEncounter();
            string physicistName = physicistData.name;
            string info;
            if (physicistTrigger.interactionCount == 0)
            {
                info = physicistData.description;
            }
            else
            {
                info = physicistData.dialogue.lines[physicistTrigger.interactionCount - 1].Text;
            }

            if (physicistTrigger.interactionCount < physicistData.dialogue.lines.Length)
            {
                physicistTrigger.interactionCount++;
            }
            else
            {
                if (physicistData.quest.questActive == false)
                {
                    if (physicistData.quest.questCompleted == false)
                    {
                        info = physicistData.quest.GetStartQuestDialogue();
                        physicistData.quest.StartQuest();
                    }
                    
                }
                else
                {
                    if (physicistData.quest.questCompleted == false)
                    {
                        bool objFound = false;
                        foreach (var obj in encounterManager.foundObjects)
                        {
                            objFound = obj == physicistData.quest.questObjective;
                        }

                        if (objFound)
                        {
                            info = physicistData.quest.GetEndQuestDialogue();
                            physicistData.quest.EndQuest();
                        }
                        else
                        {
                            info = physicistData.quest.GetMidQuestDialogue();
                        }
                    }
                }
            }
        
            PhysicistCanvas.gameObject.SetActive(true);
            Debug.Log("Attempting dialogue");
            nameText.text = physicistName;
            infoText.text = info;
        }
    }

    private void ObjectInteraction(RaycastHit hit)
    {
        
        ObjectTrigger objectTrigger = hit.transform.GetComponent<ObjectTrigger>();
        if (objectTrigger != null)
        {
            objectTrigger.TriggerEncounter();
            string objectName = objectTrigger.data.name;
            
            popupText.text = $"Encontrou e registrou um novo objeto: \n{objectName}!";
            ObjectCanvas.gameObject.SetActive(true);
        }
    }
}
