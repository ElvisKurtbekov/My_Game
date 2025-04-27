using UnityEngine;
using TMPro;

public class StatsPageManager : MonoBehaviour
{
    [SerializeField] private GameObject statsPageUI;  // UI для страницы статистики
    [SerializeField] private GameObject journalIcon;  // Иконка журнала, чтобы скрыть её при открытии страницы статистики
    [SerializeField] private TMP_Text statsText;      // Текст для отображения статистики
    [SerializeField] private GameStatsManager statsManager;  // Ссылка на GameStatsManager

    private bool isStatsPageOpen = false;  // Флаг, показывающий открыта ли страница статистики

    private void Start()
    {
        Debug.Log("z kj[");
        // Убедимся, что статистическая страница изначально скрыта
        if (statsPageUI != null)
        {
            Debug.Log("z kj[");
            statsPageUI.SetActive(false);
        }
    }

    private void Update()
    {
        // Открытие страницы статистики при нажатии на кнопку M
        if (Input.GetKeyDown(KeyCode.M))
        {
            
            ToggleStatsPage();
            
        }
    }

    private void ToggleStatsPage()
    {
        if (isStatsPageOpen)
        {
            // Закрыть страницу статистики
            statsPageUI.SetActive(false);
            isStatsPageOpen = false;

            // Возвращаем видимость иконки журнала
            UpdateJournalIconVisibility();
        }
        else
        {
            // Открыть страницу статистики
            statsPageUI.SetActive(true);
            isStatsPageOpen = true;

            // Обновляем статистику на странице
            UpdateStatsPage();

            // Скрываем иконку журнала
            UpdateJournalIconVisibility();
        }

        // Обновляем курсор
        Cursor.visible = isStatsPageOpen;
        Cursor.lockState = isStatsPageOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }

    private void UpdateStatsPage()
    {
        if (statsManager != null && statsText != null)
        {
            var stats = statsManager.GetStats();
            string statsContent = $"Принято квестов: {stats.acceptedQuests}\n" +
                                  $"Завершено квестов: {stats.completedQuests}\n" +
                                  $"Провалено квестов: {stats.failedQuests}\n" +
                                  $"Общее время игры: {stats.totalTimeSpent / 60f:F2} минут\n" +
                                  $"Общее время на квесты: {stats.totalQuestTimeSpent:F2} секунд\n" +
                                  $"Деньги заработано: {stats.totalMoneyEarned}\n" +
                                  $"Деньги потрачено: {stats.totalMoneySpent}\n" +
                                  $"Текущий баланс: {stats.currentBalance}" +
                                  $"Телепортации: {stats.teleportCount}\n";

            statsText.text = statsContent;
        }
    }


    private void UpdateJournalIconVisibility()
    {
        if (journalIcon != null)
        {
            journalIcon.SetActive(!isStatsPageOpen);
        }
    }
}
