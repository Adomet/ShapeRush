using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{

    private bool isGameOver = false;

    void OnTriggerEnter(Collider other)
    {
        if (!isGameOver)
        {
            GameManager.Instance.WinGame();
            Invoke("LoadNextLevel", 5f);
            isGameOver = true;
        }

    }

    void ReStart()
    {
        GameManager.Instance.RestartGame();
    }

    void LoadNextLevel()
    {
        GameManager.Instance.StartScene(GameManager.Instance.levelNumber+1);
    }

}
