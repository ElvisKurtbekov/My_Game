using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string title;
    public string description;
    public bool isCompleted;
    public float progress; // от 0 до 1

    public Quest(string title, string description, bool isCompleted, float progress = 0f)
    {
        this.title = title;
        this.description = description;
        this.isCompleted = isCompleted;
        this.progress = progress;
    }
}

