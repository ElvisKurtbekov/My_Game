using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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

    private bool isTyping = false; // ���� ��� ������������ �������� ������
    private float typingSpeed = 0.025f; // �������� ��������� ������

    public NPCDialogue npcDialogue; // ������ �� NPCDialogue

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void StartDialogue(string npcLine, List<string> choices, Dictionary<int, List<string>> responses)
    {
        if (dialogueUI != null)
        {
            Destroy(dialogueUI);
        }

        CreateUI();
        StartCoroutine(TypeText(npcLine)); // �������� ������ ��� ������ �������
        currentChoices = choices;
        npcResponses = responses;

        waitingForChoices = true;
        isDialogueActive = true;
    }

    private void Update()
    {
        if (isDialogueActive)
        {
            if (waitingForChoices && Input.GetMouseButtonDown(0) && !isTyping)
            {
                ShowChoices();
                waitingForChoices = false;
            }
            else if (waitingForExit && Input.GetMouseButtonDown(0) && !isTyping)
            {
                EndDialogue();
            }

            if (!waitingForChoices && !waitingForExit && showingNPCResponses)
            {
                if (Input.GetMouseButtonDown(0) && currentResponseIndex < currentResponse.Count - 1 && !isTyping)
                {
                    // ���������� ��������� ������� NPC � ���������
                    currentResponseIndex++;
                    StartCoroutine(TypeText(currentResponse[currentResponseIndex]));
                }
                else if (currentResponseIndex == currentResponse.Count - 1 && Input.GetMouseButtonDown(0) && !isTyping)
                {
                    // ��� ������� NPC ��������, ��������� ������
                    EndDialogue();
                }
            }

            // ���� ���� �����, ����������� ������� NPC
            if (!waitingForChoices && !waitingForExit && !showingNPCResponses)
            {
                if (!isTyping)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseResponse(1);
                    if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseResponse(2);
                    if (Input.GetKeyDown(KeyCode.Alpha3)) ChooseResponse(3);
                }
            }
        }
    }

    private void CreateUI()
    {
        // ������� UI-������
        dialogueUI = new GameObject("DialogueUI");
        Canvas canvas = dialogueUI.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = dialogueUI.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        dialogueUI.AddComponent<GraphicRaycaster>();

        // ��� (������)
        GameObject panelObj = new GameObject("DialoguePanel");
        panelObj.transform.SetParent(dialogueUI.transform);
        Image panelImage = panelObj.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.9f);
        RectTransform panelRect = panelObj.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0, 0);
        panelRect.anchorMax = new Vector2(1, 0.3f);
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        // ����� NPC
        GameObject textObj = new GameObject("DialogueText");
        textObj.transform.SetParent(panelObj.transform);
        dialogueText = textObj.AddComponent<TextMeshProUGUI>();
        dialogueText.fontSize = 60;
        dialogueText.alignment = TextAlignmentOptions.Center;
        dialogueText.color = Color.white;
        dialogueText.font = Resources.Load<TMP_FontAsset>("Fonts/Ponomar-Regular");

        RectTransform textRect = textObj.GetComponent<RectTransform>();
        textRect.anchorMin = new Vector2(0.1f, 0.5f);
        textRect.anchorMax = new Vector2(0.9f, 0.9f);
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // ��������� "������� ���"
        nextButtonHint = new GameObject("NextHint");
        nextButtonHint.transform.SetParent(panelObj.transform);
        TextMeshProUGUI hintText = nextButtonHint.AddComponent<TextMeshProUGUI>();
        hintText.text = "������� ���...";
        hintText.alignment = TextAlignmentOptions.Center;
        hintText.color = Color.gray;
        hintText.fontSize = 24;

        RectTransform hintRect = nextButtonHint.GetComponent<RectTransform>();
        hintRect.anchorMin = new Vector2(0.4f, 0.02f);
        hintRect.anchorMax = new Vector2(0.6f, 0.08f);
        hintRect.offsetMin = Vector2.zero;
        hintRect.offsetMax = Vector2.zero;
    }

    private IEnumerator TypeText(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in textToType)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void ShowChoices()
    {
        // ������� ������ ������� NPC
        dialogueText.text = "";

        // ������� ���������
        Destroy(nextButtonHint);

        // ������� ��������� ��� ��������� ������ ������ ������� ����
        choicesContainer = new GameObject("ChoicesContainer");
        choicesContainer.transform.SetParent(dialogueUI.transform);
        RectTransform choicesRect = choicesContainer.AddComponent<RectTransform>();

        // ��������� ��������� � �������� ������� ���� (������ ����� ������)
        choicesRect.anchorMin = new Vector2(0.3f, 0f);
        choicesRect.anchorMax = new Vector2(0.9f, 0.3f);
        choicesRect.offsetMin = Vector2.zero;
        choicesRect.offsetMax = Vector2.zero;

        float choiceHeight = 50f; // ������ ������
        float spacing = 10f; // ���������� ����� ����������
        float startY = (currentChoices.Count - 1) * (choiceHeight + spacing) / 2f; // ���������� ������ ����� �����

        // ���������� ������ � ���������� ��
        for (int i = 0; i < currentChoices.Count; i++)
        {
            GameObject buttonObj = new GameObject("ChoiceButton" + (i + 1));
            buttonObj.transform.SetParent(choicesContainer.transform);
            Button button = buttonObj.AddComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.AddComponent<TextMeshProUGUI>();

            buttonText.text = (i + 1) + ". " + currentChoices[i];
            buttonText.fontSize = 30;
            buttonText.alignment = TextAlignmentOptions.Left; // ����������� ����� �����
            buttonText.color = Color.white;
            buttonText.font = Resources.Load<TMP_FontAsset>("Fonts/Ponomar-Regular");

            RectTransform btnRect = buttonObj.GetComponent<RectTransform>();
            btnRect.anchorMin = new Vector2(0f, 0.5f);
            btnRect.anchorMax = new Vector2(1f, 0.5f);
            btnRect.pivot = new Vector2(0.5f, 0.5f);
            btnRect.sizeDelta = new Vector2(0, choiceHeight);

            btnRect.anchoredPosition = new Vector2(0, startY - i * (choiceHeight + spacing));

            int choiceIndex = i + 1;
            button.onClick.AddListener(() => ChooseResponse(choiceIndex));

            choiceButtons.Add(button);
        }
    }

    private void ChooseResponse(int choice)
    {
        if (npcResponses.ContainsKey(choice))
        {
            // ������� �������� ������
            Destroy(choicesContainer);
            choiceButtons.Clear();

            // ���� ������ ������ �������, �������� �����-�������� ��� ���������� ��������
            if (choice == 1)
            {
                UpdateNotebook("�� ������� ������ ������.");
            }

            // ���������� ������ NPC
            currentResponse = npcResponses[choice];
            currentResponseIndex = 0;
            StartCoroutine(TypeText(currentResponse[currentResponseIndex])); // �������� ������

            if (choice == 3)
            {
                EndDialogue();
                return;
            }

            showingNPCResponses = true;
        }
    }

    private void EndDialogue()
    {
        npcDialogue.EndDialogue(); // ��������� ������ �� ������� NPC
        Destroy(dialogueUI);
        isDialogueActive = false;
        waitingForExit = false;

        showingNPCResponses = false; // ���������� ���������   
    }

    private void UpdateNotebook(string entry)
    {
        Debug.Log($"������ ��������� � �������: {entry}");
    }
}