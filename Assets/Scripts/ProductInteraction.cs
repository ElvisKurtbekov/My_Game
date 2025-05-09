using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductInteraction : MonoBehaviour
{
    public string productName;  // Название товар
    public PlayerMoney playerMoney;

    private void Update()
    {
        if (IsPlayerLookingAtMe() && Input.GetKeyDown(KeyCode.E))  // Если игрок смотрит на товар и нажал 'E'
        {
            BuyProduct();
        }
    }

    private bool IsPlayerLookingAtMe()
    {
        var ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.collider.gameObject == gameObject;  // Проверяем, что объект, на который смотрит игрок, это наш товар
        }
        return false;
    }

    private void BuyProduct()
    {
        if (QuestManager.Instance != null && QuestManager.Instance.HasActiveTask())
        {
            QuestManager.Instance.currentTask.BuyItem(productName);
            Debug.Log($"Куплен товар: {productName}");
        }
        else
        {
            Debug.LogWarning("Нет активного задания для покупки товара.");
        }

        var money = PlayerMoney.Instance;
        if (money != null)
        {
            money.SpendMoney(50);
        }
        else
        {
            Debug.LogWarning("PlayerMoney.Instance не найден.");
        }
    }
}
