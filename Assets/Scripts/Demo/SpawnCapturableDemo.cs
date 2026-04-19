using UnityEngine;

public class SpawnCapturableDemo : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instantiate(Resources.Load(GameObject.Find("Manager").GetComponent<CapturedManagerDemo>().Capturable), this.transform.position, this.transform.rotation);
    }

}
