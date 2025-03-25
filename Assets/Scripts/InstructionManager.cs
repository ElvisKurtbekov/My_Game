using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class InstructionManager : MonoBehaviour
{
    [SerializeField] private NotebookManager notebookManager; // Ссылка на скрипт блокнота
    [SerializeField] private GameObject instructionUI;          // UI для страницы инструкций
    [SerializeField] private Transform instructionListContainer; // Контейнер для списка инструкций
    [SerializeField] private GameObject instructionItemPrefab;   // Префаб для одной инструкции
    [SerializeField] private TextMeshProUGUI instructionDetails; // Поле для отображения подробного описания инструкции
    [SerializeField] private GameObject journal;           // Значок журнала
    [SerializeField] private List<Instruction> instructions = new List<Instruction>(); // Список всех инструкций

    private bool isInstructionOpen = false;                // Состояние страницы инструкций (открыта/закрыта)

    private void Start()
    {
        RefreshInstructionList(); // Обновляем список инструкций при старте
    }

    private void Update()
    {
        // Открытие/закрытие инструкции по кнопке I
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInstructionUI();
        }
    }

    private void ToggleInstructionUI()
    {
        // Закрываем блокнот, если он открыт
        if (!isInstructionOpen && notebookManager.IsNotebookOpen())
        {
            notebookManager.ToggleNotebook();
        }

        // Включаем или выключаем Canvas инструкций
        if (instructionUI != null)
        {
            isInstructionOpen = !instructionUI.activeSelf;
            instructionUI.SetActive(isInstructionOpen);

            // Отображение/скрытие курсора
            Cursor.visible = isInstructionOpen;
            Cursor.lockState = isInstructionOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // Обновляем видимость значка дневника
        UpdateJournalIconVisibility();
    }

    private void RefreshInstructionList()
    {
        // Удаляем старые элементы из контейнера
        foreach (Transform child in instructionListContainer)
        {
            Destroy(child.gameObject);
        }

        // Создаем новый элемент для каждой инструкции
        foreach (Instruction instruction in instructions)
        {
            GameObject instructionItem = Instantiate(instructionItemPrefab, instructionListContainer);

            // Удаляем чекбокс, если он есть
            Transform toggleTransform = instructionItem.transform.Find("QuestToggle"); // Название объекта с чекбоксом
            if (toggleTransform != null)
            {
                Debug.Log($"Chmo&");
                Destroy(toggleTransform.gameObject);
            }

            // Устанавливаем название инструкции
            TMP_Text titleText = instructionItem.transform.Find("QuestName")?.GetComponent<TMP_Text>();
            if (titleText != null)
            {
                titleText.text = instruction.title;
            }

            // Настраиваем кнопку для отображения подробностей
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

        // Обновляем видимость значка дневника
        UpdateJournalIconVisibility();
    }
    public void UpdateJournalIconVisibility()
    {
        // Если открыт либо дневник, либо инструкции, скрываем значок
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
    public string title;       // Название инструкции
    public string description; // Подробное описание инструкции

    public Instruction(string title, string description)
    {
        this.title = title;
        this.description = description;
    }
}
