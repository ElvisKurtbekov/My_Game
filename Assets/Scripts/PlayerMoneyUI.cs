using UnityEngine;
using TMPro;

public class PlayerMoneyUI : MonoBehaviour
{
    public PlayerMoney playerMoney;
    public TextMeshProUGUI moneyText;

    void Start()
    {
        if (playerMoney == null)
        {
            playerMoney = GameObject.FindWithTag("Player").GetComponent<PlayerMoney>();
        }
        UpdateMoneyUI();
    }

    public void UpdateMoneyUI()
    {
        moneyText.text = "Δενόγθ: " + playerMoney.GetMoney().ToString();
    }
}