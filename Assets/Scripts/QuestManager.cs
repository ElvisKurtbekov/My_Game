
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public Task1 currentTask;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // сохраняется при переходе между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasActiveTask() => currentTask != null;
}
