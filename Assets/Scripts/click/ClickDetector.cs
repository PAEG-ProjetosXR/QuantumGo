using UnityEngine;

public class ClickDetector : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //cria um "raio" que sai da câmera pela posição do mouse
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                PhysicistTrigger trigger = hit.collider.GetComponent<PhysicistTrigger>();

                if (trigger != null)
                {
                    trigger.TriggerEncounter();
                }
            }
        }
    }
}