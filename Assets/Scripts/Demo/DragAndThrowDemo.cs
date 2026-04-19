

using DG.Tweening;
using UnityEngine;

public class DragAndThrowDemo : MonoBehaviour
{

    private bool dragging = false;
    private float distance;
    public float ThrowSpeed;
    public float ArchSpeed;
    public float Speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMouseDown()
    {
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;
    }
    
    public void OnMouseUp()
    {
        this.GetComponent<Rigidbody>().useGravity = true;
        this.GetComponent<Rigidbody>().linearVelocity += this.transform.forward * ThrowSpeed;
        this.GetComponent<Rigidbody>().linearVelocity += this.transform.forward * ArchSpeed;
        dragging = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = Vector3.Lerp(this.transform.position, rayPoint, Speed * Time.deltaTime);
        }
    }
}
