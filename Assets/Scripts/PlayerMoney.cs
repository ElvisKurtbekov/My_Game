using UnityEngine;

public class PlayerMoney : MonoBehaviour
{
    public int currentMoney = 0;

    public PlayerMoneyUI moneyUI; // Сюда подцепим UI

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        Debug.Log("Добавлено денег: " + amount + ". Текущий баланс: " + currentMoney);
        moneyUI?.UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
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