using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class NotebookManager : MonoBehaviour
{
    [SerializeField] private InstructionManager instructionsManager; // ������ �� ������ ����������
    [SerializeField] private GameObject notebookUI;        // UI ��������
    [SerializeField] private Transform questListContainer; // ��������� ��� ������ �������
    [SerializeField] private GameObject questItemPrefab;   // ������ ��� ������ ������� (������ � ���������)
    [SerializeField] private TextMeshProUGUI questDetails; // ���� ��� ���������� �������� �������
    [SerializeField] private GameObject journal;           // ������ �������

    private List<Quest> quests = new List<Quest>();        // ������ ���� �������
    private bool isNotebookOpen = true;                    // ��������� �������� (������/������)

    private void Start()
    {
        UpdateJournalIconVisibility();

        if (quests == null)
        {
            quests = new List<Quest>();
        }

        // ������ ��������� ������� !!!!������ ����� ���������� �������!!!!!
        quests.Add(new Quest("�������� ������� ��������� � ������� NotebookManager", "��������� �������� ��������� �������", false));
        quests.Add(new Quest("��� ���� ������� �� 26 ������ ", "�������� ������� �������", false));
        RefreshQuestList();
    }

    private void Update()
    {
        // �������/������� ������� �� ������ J
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleNotebook();     
        }
    }

    public void ToggleNotebook()
    {
        // ��������� ����������, ���� ��� �������
        if (!isNotebookOpen && instructionsManager.IsInstructionsOpen())
        {
            instructionsManager.CloseInstructions();
        }

        // �������� ��� ��������� Canvas
        if (notebookUI != null)
        {
            isNotebookOpen = !notebookUI.gameObject.activeSelf;
            notebookUI.gameObject.SetActive(isNotebookOpen);

            // �����������/������� �������
            Cursor.visible = isNotebookOpen;
            Cursor.lockState = isNotebookOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // ��������� ��������� ������ ��������
        UpdateJournalIconVisibility();
    }

    // ���������� ������ ������� � �������
    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
        RefreshQuestList();
    }

    // ���������� UI ������ �������
    private void RefreshQuestList()
    {
        // ������� ������ �������� �� ����������
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        // ������� ����� ������� ��� ������� �������
        foreach (Quest quest in quests)
        {
            GameObject questItem = Instantiate(questItemPrefab, questListContainer);

            // ������������� �������� �������
            TMP_Text titleText = questItem.transform.Find("QuestName")?.GetComponent<TMP_Text>();
            if (titleText != null)
            {
                titleText.text = quest.title;
            }

            // ������������� ��������� ��������
            Toggle questToggle = questItem.transform.Find("QuestToggle")?.GetComponent<Toggle>();
            if (questToggle != null)
            {
                questToggle.isOn = quest.isCompleted;
            }

            // ����������� ������ ��� ����������� ������������
            Button questButton = questItem.transform.Find("QuestName")?.GetComponent<Button>();
            if (questButton != null)
            {
                questButton.onClick.AddListener(() => ShowQuestDetails(quest));
            }
        }
    }

    // �������� ����������� �������
    private void ShowQuestDetails(Quest quest)
    {
        questDetails.text = quest.description;
    }

    public bool IsNotebookOpen()
    {
        return isNotebookOpen;
    }

    public void UpdateJournalIconVisibility()
    {
        // ���� ������ ���� �������, ���� ����������, �������� ������
        if (journal != null)
        {
            journal.SetActive(!isNotebookOpen && !instructionsManager.IsInstructionsOpen());
        }
    }
}
