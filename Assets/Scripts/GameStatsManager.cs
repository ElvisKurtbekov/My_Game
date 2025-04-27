using UnityEngine;

public class GameStatsManager : MonoBehaviour
{
    private int completedQuests = 0;
    private int failedQuests = 0;
    private int acceptedQuests = 0;
    private float totalTimeSpent = 0f;
    private int totalMoneyEarned = 0;
    private int totalMoneySpent = 0;
    private int teleportCount = 0;
    private float totalQuestTimeSpent = 0f;

    private float gameStartTime;

    [SerializeField] private PlayerMoney playerMoney; // чтобы брать баланс

    private void Start()
    {
        gameStartTime = Time.time;
    }

    private void Update()
    {
        totalTimeSpent = Time.time - gameStartTime;
    }

    // Методы для обновления статистики
    public void IncrementAcceptedQuest()
    {
        acceptedQuests++;
    }

    public void IncrementCompletedQuest()
    {
        completedQuests++;
    }

    public void IncrementFailedQuest()
    {
        failedQuests++;
    }

    public void AddMoneyEarned(int amount)
    {
        totalMoneyEarned += amount;
    }

    public void AddMoneySpent(int amount)
    {
        totalMoneySpent += amount;
    }

    public void IncrementTeleportCount()
    {
        teleportCount++;
    }

    public void AddQuestTime(float questTime)
    {
        totalQuestTimeSpent += questTime;
    }

    public GameStats GetStats()
    {
        int currentBalance = playerMoney != null ? playerMoney.GetMoney() : 0;
        return new GameStats(completedQuests, failedQuests, acceptedQuests, totalTimeSpent, totalMoneyEarned, totalMoneySpent, teleportCount, totalQuestTimeSpent, currentBalance);
    }
}

[System.Serializable]
public struct GameStats
{
    public int completedQuests;
    public int failedQuests;
    public int acceptedQuests;
    public float totalTimeSpent;
    public int totalMoneyEarned;
    public int totalMoneySpent;
    public int teleportCount;
    public float totalQuestTimeSpent;
    public int currentBalance;

    public GameStats(int completedQuests, int failedQuests, int acceptedQuests, float totalTimeSpent, int totalMoneyEarned, int totalMoneySpent, int teleportCount, float totalQuestTimeSpent, int currentBalance)
    {
        this.completedQuests = completedQuests;
        this.failedQuests = failedQuests;
        this.acceptedQuests = acceptedQuests;
        this.totalTimeSpent = totalTimeSpent;
        this.totalMoneyEarned = totalMoneyEarned;
        this.totalMoneySpent = totalMoneySpent;
        this.teleportCount = teleportCount;
        this.totalQuestTimeSpent = totalQuestTimeSpent;
        this.currentBalance = currentBalance;
    }
}
