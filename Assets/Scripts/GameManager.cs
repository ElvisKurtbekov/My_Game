using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Не уничтожать при смене сцены
        }
        else
        {
            Destroy(gameObject); // Удаляем дубликаты
        }
    }
}

