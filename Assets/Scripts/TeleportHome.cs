using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static PlayerSpawner;

public class TeleportHome : MonoBehaviour
{
    [SerializeField] private GameStatsManager statsManager;  // Ссылка на GameStatsManager
    [SerializeField] private string sceneName;               // Название сцены для загрузки
    public Vector3 citySpawnPoint = new Vector3(-201, 71, 17); // Координаты для спавна в городе

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Сохранение позиции перед выходом из магазина
            PlayerSpawnData.spawnPosition = other.transform.position;
            Debug.Log("Запомнена позиция перед выходом: " + PlayerSpawnData.spawnPosition);
            SceneManager.LoadScene(sceneName);
        }
    }
}
