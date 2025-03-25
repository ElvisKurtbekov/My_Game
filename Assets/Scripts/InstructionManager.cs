using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class InstructionManager : MonoBehaviour
{
    [SerializeField] private NotebookManager notebookManager; // ������ �� ������ ��������
    [SerializeField] private GameObject instructionUI;          // UI ��� �������� ����������
    [SerializeField] private Transform instructionListContainer; // ��������� ��� ������ ����������
    [SerializeField] private GameObject instructionItemPrefab;   // ������ ��� ����� ����������
    [SerializeField] private TextMeshProUGUI instructionDetails; // ���� ��� ����������� ���������� �������� ����������
    [SerializeField] private GameObject journal;           // ������ �������
    [SerializeField] private List<Instruction> instructions = new List<Instruction>(); // ������ ���� ����������

    private bool isInstructionOpen = false;                // ��������� �������� ���������� (�������/�������)

    private void Start()
    {
        RefreshInstructionList(); // ��������� ������ ���������� ��� ������
    }

    private void Update()
    {
        // ��������/�������� ���������� �� ������ I
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInstructionUI();
        }
    }

    private void ToggleInstructionUI()
    {
        // ��������� �������, ���� �� ������
        if (!isInstructionOpen && notebookManager.IsNotebookOpen())
        {
            notebookManager.ToggleNotebook();
        }

        // �������� ��� ��������� Canvas ����������
        if (instructionUI != null)
        {
            isInstructionOpen = !instructionUI.activeSelf;
            instructionUI.SetActive(isInstructionOpen);

            // �����������/������� �������
            Cursor.visible = isInstructionOpen;
            Cursor.lockState = isInstructionOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // ��������� ��������� ������ ��������
        UpdateJournalIconVisibility();
    }

    private void RefreshInstructionList()
    {
        // ������� ������ �������� �� ����������
        foreach (Transform child in instructionListContainer)
        {
            Destroy(child.gameObject);
        }

        // ������� ����� ������� ��� ������ ����������
        foreach (Instruction instruction in instructions)
        {
            GameObject instructionItem = Instantiate(instructionItemPrefab, instructionListContainer);

            // ������� �������, ���� �� ����
            Transform toggleTransform = instructionItem.transform.Find("QuestToggle"); // �������� ������� � ���������
            if (toggleTransform != null)
            {
                Debug.Log($"Chmo&");
                Destroy(toggleTransform.gameObject);
            }

            // ������������� �������� ����������
            TMP_Text titleText = instructionItem.transform.Find("QuestName")?.GetComponent<TMP_Text>();
            if (titleText != null)
            {
                titleText.text = instruction.title;
            }

            // ����������� ������ ��� ����������� ������������
            Button instructionButton = instructionItem.transform.Find("QuestName")?.GetComponent<Button>();
            if (instructionButton != null)
            {
                instructionButton.onClick.AddListener(() => ShowInstructionDetails(instruction));
            }
        }
    }

    private void ShowInstructionDetails(Instruction instruction)
    {
        instructionDetails.text = instruction.description;
    }

    public void CloseInstructions()
    {
        if (isInstructionOpen)
        {
            instructionUI.SetActive(false);
            isInstructionOpen = false;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        // ��������� ��������� ������ ��������
        UpdateJournalIconVisibility();
    }
    public void UpdateJournalIconVisibility()
    {
        // ���� ������ ���� �������, ���� ����������, �������� ������
        if (journal != null)
        {
            journal.SetActive(!notebookManager.IsNotebookOpen() && !isInstructionOpen);
        }
    }

    public bool IsInstructionsOpen()
    {
        return isInstructionOpen;
    }

}

[System.Serializable]
public class Instruction
{
    public string title;       // �������� ����������
    public string description; // ��������� �������� ����������

    public Instruction(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}
