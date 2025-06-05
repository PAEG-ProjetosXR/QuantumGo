using Fog.Dialogue;
using Malee.List;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "QuantumGo/QuestData")]
public class QuestData : ScriptableObject
{
    public PhysicistData questGiver;
    public ObjectData questObjective;
    
    public string description;
    
    public bool questActive = false;
    public bool questCompleted = false;
    
    [SerializeField, TextArea(3, 5)] public string startQuestText;
    [SerializeField, TextArea(3, 5)] public string midQuestText;
    [SerializeField, TextArea(3, 5)] public string endQuestText;

    public string GetStartQuestDialogue()
    {
        return startQuestText;
    }

    public string GetMidQuestDialogue()
    {
        return midQuestText;
    }

    public string GetEndQuestDialogue()
    {
        return endQuestText;
    }

    public void StartQuest()
    {
        questActive = true;
    }

    public void EndQuest()
    {
        questActive = false;
        questCompleted = true;
    }
}
