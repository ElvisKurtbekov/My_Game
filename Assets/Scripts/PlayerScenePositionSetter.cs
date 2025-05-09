using UnityEngine;
using static PlayerSpawner;

public class PlayerScenePositionSetter : MonoBehaviour
{
    [SerializeField] private Vector3 defaultStartPosition = new Vector3(-201, 71, 17); // или любые нужные координаты

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Vector3 targetPosition = PlayerSpawnData.spawnPosition;

            // Проверка: если позиция не задана — используем дефолтную
            if (targetPosition == Vector3.zero)
            {
                targetPosition = defaultStartPosition;
            }

            player.transform.position = targetPosition;
        }
        else
        {
            Debug.LogWarning("Игрок не найден в сцене для телепортации.");
        }
    }
}
