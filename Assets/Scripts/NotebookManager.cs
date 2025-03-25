using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;

public class NotebookManager : MonoBehaviour
{
    [SerializeField] private InstructionManager instructionsManager; // Ссылка на скрипт инструкций
    [SerializeField] private GameObject notebookUI;        // UI блокнота
    [SerializeField] private Transform questListContainer; // Контейнер для списка заданий
    [SerializeField] private GameObject questItemPrefab;   // Префаб для одного задания (строка с чекбоксом)
    [SerializeField] private TextMeshProUGUI questDetails; // Поле для подробного описания задания
    [SerializeField] private GameObject journal;           // Значок журнала

    private List<Quest> quests = new List<Quest>();        // Список всех заданий
    private bool isNotebookOpen = true;                    // Состояние блокнота (открыт/закрыт)

    private void Start()
    {
        UpdateJournalIconVisibility();

        if (quests == null)
        {
            quests = new List<Quest>();
        }

        // Пример тестового задания !!!!УБРАТЬ ПОСЛЕ ДОБАВЛЕНИЯ ЗАДАНИЙ!!!!!
        quests.Add(new Quest("Тестовое задание находится в скрипте NotebookManager", "Подробное описание тестового задания", false));
        quests.Add(new Quest("Еще одно задание на 26 строке ", "Описание второго задания", false));
        RefreshQuestList();
    }

    private void Update()
    {
        // Открыть/закрыть блокнот по кнопке J
        if (Input.GetKeyDown(KeyCode.J))
        {
            ToggleNotebook();     
        }
    }

    public void ToggleNotebook()
    {
        // Закрываем инструкции, если они открыты
        if (!isNotebookOpen && instructionsManager.IsInstructionsOpen())
        {
            instructionsManager.CloseInstructions();
        }

        // Включаем или выключаем Canvas
        if (notebookUI != null)
        {
            isNotebookOpen = !notebookUI.gameObject.activeSelf;
            notebookUI.gameObject.SetActive(isNotebookOpen);

            // Отображение/скрытие курсора
            Cursor.visible = isNotebookOpen;
            Cursor.lockState = isNotebookOpen ? CursorLockMode.None : CursorLockMode.Locked;
        }

        // Обновляем видимость значка дневника
        UpdateJournalIconVisibility();
    }

    // Добавление нового задания в блокнот
    public void AddQuest(Quest newQuest)
    {
        quests.Add(newQuest);
        RefreshQuestList();
    }

    // Обновление UI списка заданий
    private void RefreshQuestList()
    {
        // Удаляем старые элементы из контейнера
        foreach (Transform child in questListContainer)
        {
            Destroy(child.gameObject);
        }

        // Создаем новый элемент для каждого задания
        foreach (Quest quest in quests)
        {
            GameObject questItem = Instantiate(questItemPrefab, questListContainer);

            // Устанавливаем название задания
            TMP_Text titleText = questItem.transform.Find("QuestName")?.GetComponent<TMP_Text>();
            if (titleText != null)
            {
                titleText.text = quest.title;
            }

            // Устанавливаем состояние чекбокса
            Toggle questToggle = questItem.transform.Find("QuestToggle")?.GetComponent<Toggle>();
            if (questToggle != null)
            {
                questToggle.isOn = quest.isCompleted;
            }

            // Настраиваем кнопку для отображения подробностей
            Button questButton = questItem.transform.Find("QuestName")?.GetComponent<Button>();
            if (questButton != null)
            {
                questButton.onClick.AddListener(() => ShowQuestDetails(quest));
            }
        }
    }

    // Показать подробности задания
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
        // Если открыт либо дневник, либо инструкции, скрываем значок
        if (journal != null)
        {
            journal.SetActive(!isNotebookOpen && !instructionsManager.IsInstructionsOpen());
        }
    }
}
