using UnityEngine;
using System.Collections.Generic;
using TMPro;

[System.Serializable]
public class NPCResponse
{
    [TextArea(3, 5)]
    public List<string> responses; // Список реплик NPC
}

public class NPCDialogue : MonoBehaviour
{
    [TextArea(3, 5)]
    public string npcLine;

    [TextArea(2, 2)]
    public List<string> playerChoices = new List<string>();
    public List<NPCResponse> npcResponses = new List<NPCResponse>();
    public float interactionDistance = 3f;
    private bool isDialogueActive = false;
    [SerializeField] private GameObject interactionHint; // Ссылка на текст подсказки

    private bool isPlayerNearby = false; // Флаг для отслеживания близости игрока
    private float hintHideDelay = 0.2f;  // Задержка перед скрытием подсказки
    private float timeSinceLastSeen = 0f;

    [SerializeField] private Quest assignedQuest; // Задание, связанное с NPC


    private void Start()
    {
        // Проверяем, назначен ли объект InteractionHint
        if (interactionHint != null)
        {
            interactionHint.SetActive(false); // Скрываем подсказку в начале
        }
        else
        {
            Debug.LogError("Interaction hint text not assigned in the Inspector!");
        }
    }

    private void Update()
    {
        // Если диалог активен, скрываем подсказку и не проверяем условия
        if (isDialogueActive)
        {
            HideHint();
            return;
        }

        // Проверяем, находится ли игрок в зоне взаимодействия
        bool playerLookingAtMe = IsPlayerLookingAtMe();

        if (playerLookingAtMe)
        {
            ShowHint();
            isPlayerNearby = true;
            timeSinceLastSeen = 0f; // Сбрасываем таймер, если игрок видит NPC
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

        // Проверяем нажатие "E" для начала диалога
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }
    }

    private bool IsPlayerLookingAtMe()
    {
        Camera mainCamera = Camera.main;
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        return Physics.Raycast(ray, out hit, interactionDistance) && hit.collider.gameObject == gameObject;
    }

    private void ShowHint()
    {
        if (interactionHint != null && !interactionHint.activeSelf)
        {
            interactionHint.SetActive(true);
        }
    }

    private void HideHint()
    {
        if (interactionHint != null && interactionHint.activeSelf)
        {
            interactionHint.SetActive(false);
        }
    }

    public void StartDialogue()
    {

        NotebookManager notebook = FindObjectOfType<NotebookManager>();
        notebook.AddQuest(assignedQuest);

        // Логика запуска диалога
        if (isDialogueActive) return;

        // Скрываем подсказку при начале диалога
        HideHint();

        Dictionary<int, List<string>> responseDict = new Dictionary<int, List<string>>();
        for (int i = 0; i < playerChoices.Count; i++)
        {
            if (i < npcResponses.Count)
            {
                responseDict.Add(i + 1, npcResponses[i].responses);
            }
        }

        DialogueManager dialogueManager = FindObjectOfType<DialogueManager>();
        if (dialogueManager != null)
        {
            isDialogueActive = true;
            dialogueManager.StartDialogue(npcLine, playerChoices, responseDict);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
    }

    public void AddingTask()
    {

    }
}
