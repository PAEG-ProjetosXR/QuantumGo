using TMPro;
using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public TMP_Text popupText;

    public TMP_Text nameText;
    public TMP_Text infoText;
    
    public static Canvas canvas;
    
    public Canvas PhysicistCanvas;
    public Canvas ObjectCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        if (physicistTrigger != null)
        {
            physicistTrigger.TriggerEncounter();
            string physicistName = physicistTrigger.data.name;
            string info;
            if (physicistTrigger.interactionCount == 0)
            {
                info = physicistTrigger.data.description;
            }
            else
            {
                info = physicistTrigger.data.dialogue.lines[physicistTrigger.interactionCount - 1].Text;
            }

            if (physicistTrigger.interactionCount < physicistTrigger.data.dialogue.lines.Length)
            {
                physicistTrigger.interactionCount++;
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
