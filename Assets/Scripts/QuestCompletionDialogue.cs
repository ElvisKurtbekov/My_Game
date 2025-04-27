using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestCompletionDialogue : MonoBehaviour
{
    [Header("Dependencies")]
    public Animator npcAnimator;
    public Task1 task; // ссылка на Task1, чтобы получать параметры выполнения задания

    private GameObject dialogueUI;
    private TextMeshProUGUI dialogueText;
    private GameObject nextButtonHint;

    private List<string> currentDialogue = new List<string>();
    private int currentLine = 0;
    private float typingSpeed = 0.025f;
    private bool isTyping = false;
    private bool isDialogueActive = false;

    [SerializeField] private GameStatsManager statsManager;  // Ссылка на GameStatsManager

    public void StartCompletionDialogue()
    {
        if (!task) return;

        InitUI();
        npcAnimator?.SetBool("IsTalking", true);
        currentDialogue = GenerateResponseBasedOnTask();
        currentLine = 0;
        isDialogueActive = true;

        StartCoroutine(TypeText(currentDialogue[currentLine]));
    }

    private void Update()
    {
        if (!isDialogueActive || isTyping) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (currentLine < currentDialogue.Count - 1)
            {
                currentLine++;
                StartCoroutine(TypeText(currentDialogue[currentLine]));
            }
            else
            {
                EndDialogue();
            }
        }
    }

    private void InitUI()
    {
        if (dialogueUI != null) Destroy(dialogueUI);

        dialogueUI = new GameObject("QuestDialogueUI");
        var canvas = dialogueUI.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        var scaler = dialogueUI.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        dialogueUI.AddComponent<GraphicRaycaster>();

        GameObject panel = new GameObject("DialoguePanel", typeof(Image));
        panel.transform.SetParent(dialogueUI.transform);
        var panelImg = panel.GetComponent<Image>();
        panelImg.sprite = Resources.Load<Sprite>("DialogueBG");
        panelImg.color = Color.white;
        var rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 0);
        rect.anchorMax = new Vector2(1, 0.3f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        GameObject textObj = new GameObject("DialogueText", typeof(TextMeshProUGUI));
        textObj.transform.SetParent(panel.transform);
        dialogueText = textObj.GetComponent<TextMeshProUGUI>();
        dialogueText.fontSize = 60;
        dialogueText.alignment = TextAlignmentOptions.Center;
        dialogueText.color = Color.white;
        dialogueText.font = Resources.Load<TMP_FontAsset>("Fonts/DearType SDF");
        var textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.1f, 0.1f);
        textRect.anchorMax = new Vector2(0.9f, 0.9f);
        textRect.offsetMin = textRect.offsetMax = Vector2.zero;

        nextButtonHint = new GameObject("NextHint", typeof(TextMeshProUGUI));
        nextButtonHint.transform.SetParent(panel.transform);
        var hintText = nextButtonHint.GetComponent<TextMeshProUGUI>();
        hintText.text = "Нажмите ЛКМ...";
        hintText.alignment = TextAlignmentOptions.Center;
        hintText.color = Color.gray;
        hintText.fontSize = 24;
        var hintRect = nextButtonHint.GetComponent<RectTransform>();
        hintRect.anchorMin = new Vector2(0.4f, 0.02f);
        hintRect.anchorMax = new Vector2(0.6f, 0.08f);
        hintRect.offsetMin = hintRect.offsetMax = Vector2.zero;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private List<string> GenerateResponseBasedOnTask()
    {
        List<string> response = new List<string>();
        Debug.Log($"[DEBUG] Correct: {task.CompletedCorrectly}, TimeSpent: {task.TimeSpent}, Allowed: {task.AllowedTime}");

        if (task.CompletedCorrectly && task.TimeSpent <= task.AllowedTime)
        {
            response.Add("Отличная работа! Ты справился быстро и без ошибок.");
            response.Add("Можешь быть свободен. Дальше будет только сложнее!");
            statsManager.IncrementCompletedQuest();
        }
        else if (task.CompletedCorrectly && task.TimeSpent > task.AllowedTime)
        {
            response.Add("Хм, ты выполнил всё правильно, но потратил слишком много времени.");
            response.Add("В следующий раз постарайся действовать быстрее.");
        }
        else if (!task.CompletedCorrectly)
        {
            response.Add("Нет, так дело не пойдет.");
            response.Add("Ты допустил слишком много ошибок. Попробуй снова.");
            task.ResetTask();
            statsManager.IncrementFailedQuest();
        }

        return response;
    }

    private void EndDialogue()
    {
        npcAnimator?.SetBool("IsTalking", false);
        isDialogueActive = false;
        Destroy(dialogueUI);
        task.npcDialogue.EndDialogue(); // сбрасываем флаг в NPC
        FindObjectOfType<NotebookManager>().CompleteQuestWithResult(task.questTitle, task);

    }
}
