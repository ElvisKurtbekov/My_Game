using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [SerializeField] private GameStatsManager statsManager;  // —сылка на GameStatsManager

    private void OnTriggerEnter(Collider other)
    {
        statsManager.IncrementTeleportCount();
        SceneManager.LoadScene("City");
    }
}
