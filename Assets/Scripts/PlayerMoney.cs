using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney Instance { get; private set; }

    public int currentMoney = 5000;

    private PlayerMoneyUI moneyUI;
    private GameStatsManager statsManager;

    private void Awake()
    {
        // Singleton и DontDestroy
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        moneyUI = FindObjectOfType<PlayerMoneyUI>();
        statsManager = FindObjectOfType<GameStatsManager>();
    }

    private void OnLevelWasLoaded(int level)
    {
        // Попробуй снова найти UI, если сцена сменилась
        moneyUI = FindObjectOfType<PlayerMoneyUI>();
        statsManager = FindObjectOfType<GameStatsManager>();
        moneyUI?.UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Добавлено денег: " + amount + ". Текущий баланс: " + currentMoney);
        moneyUI?.UpdateMoneyUI();
        statsManager?.AddMoneyEarned(amount);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log("Потрачено денег: " + amount + ". Осталось: " + currentMoney);
            moneyUI?.UpdateMoneyUI();
            statsManager?.AddMoneySpent(amount);
            return true;
        }
        else
        {
            Debug.Log("Недостаточно денег!");
            return false;
        }
    }

    public int GetMoney() => currentMoney;
}
