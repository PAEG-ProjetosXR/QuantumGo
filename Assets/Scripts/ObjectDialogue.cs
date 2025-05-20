using Fog.Dialogue;
using UnityEngine;

public class ObjectDialogue : MonoBehaviour, IInteractable
{
    [SerializeField] private Dialogue dialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public void OnInteractAttempt()
    {
        if (dialogue != null)
        {
            DialogueHandler.instance.StartDialogue(dialogue);
        }
    }
}
