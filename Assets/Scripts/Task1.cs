using UnityEngine;

public class Task1 : MonoBehaviour
{
    public string questTitle;
    [TextArea(3, 5)]
    public string description;
    public bool isCompleted = false;
    public NPCDialogue npcDialogue;

    public bool CompletedCorrectly = true;
    public float TimeSpent = 40f;
    public float AllowedTime = 60f; // к примеру, 60 секунд

    [SerializeField] private GameStatsManager statsManager; // Добавляем ссылку

    private void Start()
    {
        //CompleteQuest();
    }

    public void CompleteQuest()
    {
        //условия для выполнения задания, надо записать время за которое сделано задание для статистики
        isCompleted = true;
        Debug.Log($"Квест завершён: {questTitle}");

        //Замеряешь время и закидываешь его в этот метод
        statsManager?.AddQuestTime(TimeSpent);
    }
    public void ResetTask()
    {
        //если задание неправильно, то перезапускаем
        isCompleted = true;
        CompletedCorrectly = false;
        TimeSpent = 0f;
        Debug.Log("Задание сброшено. Попробуй снова.");
    }

    public bool IsCompleted() => isCompleted;
}