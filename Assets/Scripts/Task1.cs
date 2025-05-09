using UnityEngine;
using System.Collections.Generic;

public class Task1 : MonoBehaviour
{
    public string questTitle = "Поход в магазин";
    [TextArea(3, 5)]
    public string description = "Пойди в магазин, купи продукты и вернись домой.";
    public bool isCompleted = false;
    public NPCDialogue npcDialogue;

    public List<string> requiredItems = new List<string> { "Хлеб", "Молоко", "Яйца" };  // Продукты для покупки
    private List<string> purchasedItems = new List<string>();  // Купленные товары

    public bool CompletedCorrectly = true;
    public float TimeSpent = 40f;  // Время для выполнения задания
    public float AllowedTime = 60f; // Допустимое время

    [SerializeField] private GameStatsManager statsManager;

    private void Start()
    {
        isCompleted = false;
    }

    // Покупка товара
    public void BuyItem(string itemName)
    {
        if (!purchasedItems.Contains(itemName) && requiredItems.Contains(itemName))
        {
            purchasedItems.Add(itemName);
            Debug.Log($"Куплен товар: {itemName}");

            // Проверяем, все ли товары куплены
            if (purchasedItems.Count == requiredItems.Count)
            {
                CompleteQuest();
            }
        }
    }

    // Завершение квеста
    public void CompleteQuest()
    {
        if (isCompleted) return;

        isCompleted = true;
        Debug.Log($"Квест завершён: {questTitle}");

        // Замер времени
        statsManager?.AddQuestTime(TimeSpent);

        // Логика завершения квеста, например, взаимодействие с NPC
        npcDialogue?.EndDialogue();

        // Добавление квеста в журнал
        FindObjectOfType<NotebookManager>().CompleteQuestWithResult(questTitle, this);
    }

    // Сброс задания
    public void ResetTask()
    {
        isCompleted = false;
        CompletedCorrectly = false;
        purchasedItems.Clear();
        TimeSpent = 0f;
        Debug.Log("Задание сброшено. Попробуй снова.");
    }

    // Проверка выполнения задания
    public bool IsCompleted() => isCompleted;

    // Получить список купленных товаров
    public List<string> GetPurchasedItems() => purchasedItems;
}
