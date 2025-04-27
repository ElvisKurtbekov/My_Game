using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int currentMoney = 0;

    public PlayerMoneyUI moneyUI; // Сюда подцепим UI

    [SerializeField] private GameStatsManager statsManager;  // Ссылка на GameStatsManager

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Добавлено денег: " + amount + ". Текущий баланс: " + currentMoney);
        moneyUI?.UpdateMoneyUI();
        statsManager.AddMoneyEarned(amount);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            statsManager.AddMoneySpent(amount);
            Debug.Log("Потрачено денег: " + amount + ". Осталось: " + currentMoney);
            moneyUI?.UpdateMoneyUI();
            return true;
        }
        else
        {
            Debug.Log("Недостаточно денег!");
            return false;
        }
    }
    public int GetMoney()
    {
        return currentMoney;
    }

}