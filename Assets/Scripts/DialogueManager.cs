using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    private static DialogueManager instance;

    private GameObject dialogueUI;
    private TextMeshProUGUI dialogueText;
    private GameObject nextButtonHint;
    private GameObject choicesContainer;
    private List<Button> choiceButtons = new List<Button>();

    private List<string> currentChoices = new List<string>();
    private Dictionary<int, List<string>> npcResponses = new Dictionary<int, List<string>>();
    private List<string> currentResponse = new List<string>();
    private int currentResponseIndex = 0;

    private bool isDialogueActive = false;
    private bool waitingForChoices = false;
    private bool waitingForExit = false;
    private bool showingNPCResponses = false;
    private bool isTyping = false;

    private float typingSpeed = 0.025f;

    public NPCDialogue npcDialogue;
    public QuestCompletionDialogue questComplete;
    public Animator npcAnimator;
    

    [SerializeField] private GameStatsManager statsManager; // Добавляем вверху

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(string npcLine, List<string> choices, Dictionary<int, List<string>> responses)
    {
        if (dialogueUI != null) Destroy(dialogueUI);

        CreateUI();
        npcAnimator?.SetBool("IsTalking", true);
        StartCoroutine(TypeText(npcLine));

        currentChoices = choices;
        npcResponses = responses;
        waitingForChoices = true;
        isDialogueActive = true;
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        if (waitingForChoices && Input.GetMouseButtonDown(0) && !isTyping)
        {
            ShowChoices();
            waitingForChoices = false;
        }
        else if (waitingForExit && Input.GetMouseButtonDown(0) && !isTyping)
        {
            EndDialogue();
        }
        else if (showingNPCResponses && Input.GetMouseButtonDown(0) && currentResponseIndex < currentResponse.Count - 1 && !isTyping)
        {
            currentResponseIndex++;
            StartCoroutine(TypeText(currentResponse[currentResponseIndex]));
        }
        else if (showingNPCResponses && currentResponseIndex == currentResponse.Count - 1 && Input.GetMouseButtonDown(0) && !isTyping)
        {
            EndDialogue();
        }

        if (!waitingForChoices && !waitingForExit && !showingNPCResponses && !isTyping)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseResponse(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseResponse(2);
            if (Input.GetKeyDown(KeyCode.Alpha3)) ChooseResponse(3);
        }
    }

    private void CreateUI()
    {
        dialogueUI = new GameObject("DialogueUI");
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

    private void ShowChoices()
    {
        npcAnimator?.SetBool("IsTalking", false);
        Destroy(nextButtonHint);
        dialogueText.text = "";

        choicesContainer = new GameObject("ChoicesContainer");
        choicesContainer.transform.SetParent(dialogueUI.transform);
        var rect = choicesContainer.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.3f, 0f);
        rect.anchorMax = new Vector2(0.9f, 0.3f);
        rect.offsetMin = rect.offsetMax = Vector2.zero;

        float height = 50f;
        float spacing = 10f;
        float startY = (currentChoices.Count - 1) * (height + spacing) / 2f;

        for (int i = 0; i < currentChoices.Count; i++)
        {
            var buttonObj = new GameObject("ChoiceButton" + (i + 1));
            buttonObj.transform.SetParent(choicesContainer.transform);
            var button = buttonObj.AddComponent<Button>();
            var text = buttonObj.AddComponent<TextMeshProUGUI>();

            text.text = $"{i + 1}. {currentChoices[i]}";
            text.fontSize = 60;
            text.alignment = TextAlignmentOptions.Left;
            text.color = Color.white;
            text.font = Resources.Load<TMP_FontAsset>("Fonts/DearType SDF");

            var btnRect = buttonObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0f, 0.5f);
            btnRect.anchorMax = new Vector2(1f, 0.5f);
            btnRect.pivot = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(0, height);
            btnRect.anchoredPosition = new Vector2(0, startY - i * (height + spacing));

            int choiceIndex = i + 1;
            button.onClick.AddListener(() => ChooseResponse(choiceIndex));
            choiceButtons.Add(button);
        }
    }

    private void ChooseResponse(int choice)
    {
        npcAnimator?.SetBool("IsTalking", true);

        if (!npcResponses.ContainsKey(choice)) return;

        Destroy(choicesContainer);
        choiceButtons.Clear();

        if (npcDialogue && choice == 1)
        {
            npcDialogue.LogEntry(); // NPC сам решает, логировать ли квест
            statsManager?.IncrementAcceptedQuest(); // Добавляем сюда, когда выбрали 1
        }

        currentResponse = npcResponses[choice];
        currentResponseIndex = 0;
        StartCoroutine(TypeText(currentResponse[currentResponseIndex]));

        if (choice == 3)
        {
            EndDialogue();
        }
        else if (currentResponseIndex == currentResponse.Count - 1)
        {
            waitingForExit = true;
        }

        showingNPCResponses = true;
    }

    private void EndDialogue()
    {
        npcDialogue?.EndDialogue();

        Destroy(dialogueUI);
        isDialogueActive = false;
        waitingForExit = false;
        showingNPCResponses = false;

        npcAnimator?.SetBool("IsTalking", false);
    }
}
