using System.Collections;
using UnityEngine;
using Fog.Dialogue;

public class CatchManagerDemo : MonoBehaviour
{
    public bool HasCaught = false;
    static float CAMERA_FOV = 8.2f;
    void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.tag == "Physicist") || (other.gameObject.tag == "Object"))
        {
            StartCoroutine(CatchObject( other.gameObject, other.gameObject.GetComponent<MainCapturable>().isObject, other.gameObject.GetComponent<MainCapturable>().id, other.gameObject.GetComponent<MainCapturable>().nome, other.gameObject.GetComponent<MainCapturable>().description, other.gameObject.GetComponent<MainCapturable>().dialogue, other.gameObject.GetComponent<MainCapturable>().quest, other.gameObject.GetComponent<MainCapturable>().icon, other.gameObject.GetComponent<MainCapturable>().modelPrefab, other.gameObject.GetComponent<MainCapturable>().presentationAudio, other.gameObject.GetComponent<MainCapturable>().correctIcon));
        }
    }

    IEnumerator CatchObject(GameObject FisOrObj, bool newIsObject, int newId, string newName, string newDescription, Dialogue newDialogue, QuestData newQuest, Sprite newIcon, GameObject newModel, AudioClip newAudio, ObjectData newCorrectIcon)
    {
        this.GetComponent<Rigidbody>().useGravity = false;
        this.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        Destroy(FisOrObj.gameObject);
        yield return new WaitForSeconds(1);
        GameObject.FindGameObjectWithTag("PlayerView").transform.LookAt(this.transform);
        GameObject.FindGameObjectWithTag("PlayerView").GetComponent<Camera>().fieldOfView = CAMERA_FOV;
        //Atomo amplia até reduzir e testa captura
        yield return new WaitForSeconds(1);
        //reduz e amplia
        //HasCaught = true;

        GameObject.Find("Manager").GetComponent<CapturedManagerDemo>().GrabCapturable(newIsObject, newId, newName, newDescription, newDialogue, newQuest, newIcon, newModel, newAudio, newCorrectIcon);
    }
}
