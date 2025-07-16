using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Player Setup")]
    public Transform player;
    public int lives = 2;

    [Header("Game Bounds")]
    public Vector3 boundsCenter = Vector3.zero;
    public Vector3 boundsSize = new(20, 15, 20);

    [Header("UI")]
    public GameObject gameOverPanel;
    public HUDManager hudManager;

    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!IsInsideBounds(player.position))
        {
            HandlePlayerOutOfBounds();
        }
    }

    bool IsInsideBounds(Vector3 posPlayer)
    {
        Vector3 min = boundsCenter - boundsSize / 2;
        Vector3 max = boundsCenter + boundsSize / 2;

        return posPlayer.x > min.x && posPlayer.x < max.x &&
                posPlayer.y > min.y && posPlayer.y < max.y &&
                posPlayer.z > min.z && posPlayer.z < max.z;
    }

    void HandlePlayerOutOfBounds()
    {
        lives--;

        if (hudManager != null)
        {
            hudManager.SetLives(lives);
        }

        if (lives <= 0)
        {
            // Todo: RespawnPlayer function delays to catch gameover.
            RespawnPlayer();
            GameOver();
        }
        else
        {
            RespawnPlayer();
        }
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    void RespawnPlayer()
    {
        player.position = Vector3.zero;
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;

        // Todo: animation/effects
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(boundsCenter, boundsSize);
    }
}
