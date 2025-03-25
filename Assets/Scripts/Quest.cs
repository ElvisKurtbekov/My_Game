using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;          // Заголовок задания
    public string description;    // Подробное описание задания
    public bool isCompleted;      // Статус выполнения задания

    // Конструктор, который принимает 3 параметра
    public Quest(string title, string description, bool isCompleted)
    {
        this.title = title;
        this.description = description;
        this.isCompleted = isCompleted;
    }
}
