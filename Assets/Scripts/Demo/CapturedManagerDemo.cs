using UnityEngine;
using System.Collections.Generic;
using Fog.Dialogue;

public class CapturedManagerDemo : MonoBehaviour
{
    const string OBJETO = "Objeto";
    const string FISICO = "Fisico";
    List<NewCapturableDemo> physicist = new List<NewCapturableDemo>();
    List<NewCapturableDemo> objeto = new List<NewCapturableDemo>();
    public string Capturable;

    void Start()
    {
        Object.DontDestroyOnLoad(this.gameObject);
    }

    public void GrabCapturable(bool newIsObject, int newId, string newName, string newDescription, Dialogue newDialogue, QuestData newQuest, Sprite newIcon, GameObject newModel, AudioClip newAudio, ObjectData newCorrectIcon)
    {
        if(newIsObject)
        {
            objeto.Add(new NewCapturableDemo(newIsObject, newId, newName, newDescription, newDialogue, newQuest, newIcon, newModel, newAudio, newCorrectIcon));
        }
        else
        {
            physicist.Add(new NewCapturableDemo(newIsObject, newId, newName, newDescription, newDialogue, newQuest, newIcon, newModel, newAudio, newCorrectIcon));   
        }
        
        Debug.Log(newName);
    }
}
