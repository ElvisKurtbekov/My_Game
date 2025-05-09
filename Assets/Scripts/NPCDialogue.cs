using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class NPCResponse
{
    [TextArea(3, 5)]
    public List<string> responses;
}

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 5)]
    public string npcLine;
    public List<string> playerChoices = new();
    public List<NPCResponse> npcResponses = new();

    [SerializeField] private GameObject interactionHint;
    [SerializeField] private Quest assignedQuest;
    [SerializeField] private Task1 taskResult;
    [SerializeField] private QuestCompletionDialogue questComplete;

    private bool isDialogueActive = false;
    private bool isPlayerNearby = false;
    private bool hasQuestBeenLogged = false;
    private float interactionDistance = 3f;
    private float hintHideDelay = 0.2f;
    private float timeSinceLastSeen;

    private void Start()
    {
        if (interactionHint != null)
            interactionHint.SetActive(false);
        else
            Debug.LogError("Interaction hint text not assigned!");
    }

    private void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E) && taskResult.IsCompleted())
        {
            HideHint();
            questComplete.StartCompletionDialogue();
            isDialogueActive = true;
            return;
        }

        if (isDialogueActive) { HideHint(); return; }

        bool playerLooking = IsPlayerLookingAtMe();
        if (playerLooking)
        {
            ShowHint();
            isPlayerNearby = true;
            timeSinceLastSeen = 0f;
        }
        else
        {
            timeSinceLastSeen += Time.deltaTime;
            if (timeSinceLastSeen >= hintHideDelay)
            {
                HideHint();
                isPlayerNearby = false;
            }
        }

        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
            StartDialogue();
    }

    private bool IsPlayerLookingAtMe()
    {
        var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        return Physics.Raycast(ray, out var hit, interactionDistance) && hit.collider.gameObject == gameObject;
    }

    public void ShowHint() { if (interactionHint && !interactionHint.activeSelf) interactionHint.SetActive(true); }
    private void HideHint() { if (interactionHint && interactionHint.activeSelf) interactionHint.SetActive(false); }

    public void StartDialogue()
    {
        if (!hasQuestBeenLogged && assignedQuest != null)
        {
            QuestManager.Instance.currentTask = taskResult;
            hasQuestBeenLogged = true;

            var notebook = FindObjectOfType<NotebookManager>();
            notebook.AddQuest(assignedQuest);
        }
        if (isDialogueActive) return;

        HideHint();

        var responseDict = new Dictionary<int, List<string>>();
        for (int i = 0; i < playerChoices.Count && i < npcResponses.Count; i++)
        {
            responseDict.Add(i + 1, npcResponses[i].responses);
        }

        var manager = FindObjectOfType<DialogueManager>();
        if (manager)
        {
            isDialogueActive = true;
            manager.npcDialogue = this;
            manager.questComplete = null;
            manager.StartDialogue(npcLine, playerChoices, responseDict);
        }
    }

    public void EndDialogue() => isDialogueActive = false;

    public void LogEntry()
    {
        if (!hasQuestBeenLogged && assignedQuest != null)
        {
            var notebook = FindObjectOfType<NotebookManager>();
            notebook.AddQuest(assignedQuest);
            hasQuestBeenLogged = true;
        }
    }
}