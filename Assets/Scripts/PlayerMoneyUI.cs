using UnityEngine;
using TMPro;

public class PlayerMoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private PlayerMoney playerMoney;

    void Start()
    {
        playerMoney = PlayerMoney.Instance;

        if (playerMoney == null)
        {
            Debug.LogError("PlayerMoney.Instance не найден!");
            return;
        }

        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        if (moneyText == null || playerMoney == null)
        {
            Debug.LogWarning("moneyText или playerMoney не назначены.");
            return;
        }

        moneyText.text = "Деньги: " + playerMoney.GetMoney().ToString();
        Debug.Log("Обновлен баланс UI: " + moneyText.text);
    }
}
