using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Product : MonoBehaviour
{
    public string productName;  // Название товара
    public Task1 task;  // Ссылка на квест

    private void OnMouseDown()
    {
        // Когда игрок нажимает на объект товара (когда камера смотрит на товар)
        if (task != null)
        {
            task.BuyItem(productName);  // Покупаем товар
        }
    }
}
