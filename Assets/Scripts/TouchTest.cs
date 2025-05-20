using UnityEngine;

public class TouchTest : MonoBehaviour
{
    public GameObject popup;
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
                
                if (hit.transform.CompareTag("Object"))
                {
                    Debug.Log(hit.transform.name + " : " + hit.transform.tag);
                    ObjectDialogue od = hit.transform.GetComponent<ObjectDialogue>();
                    if (od != null)
                    {
                        Debug.Log("Attempting dialogue");
                        od.OnInteractAttempt();
                    }
                }
            }
        }
    }
}
