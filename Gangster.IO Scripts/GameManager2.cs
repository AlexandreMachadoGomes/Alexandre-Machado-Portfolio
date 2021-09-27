using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager2 : MonoBehaviour
{

    public int GameOverSceneNumber;
    public int GameWinSceneNumber;
    public int MainGameSceneNumber;
    public int GameMenuSceneNumber;

    private float normalTime;
    private float normalFixedTime;

    public List<Enemy> enemies;

    private void Start()
    {
        //Screen.SetResolution()
        normalTime = Time.timeScale;
        normalFixedTime = Time.fixedDeltaTime;
    }

    public void GameOver()
    {
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                enemies[i].dead = true;
            }
        }

        Time.timeScale = normalTime;
        Time.fixedDeltaTime = normalFixedTime;
        SceneManager.LoadSceneAsync(GameOverSceneNumber, LoadSceneMode.Single);
    }

    public void GameWin()
    {
        Time.timeScale = normalTime;
        Time.fixedDeltaTime = normalFixedTime;
        SceneManager.LoadSceneAsync(GameWinSceneNumber, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        Time.timeScale = normalTime;
        Time.fixedDeltaTime = normalFixedTime;
        SceneManager.LoadSceneAsync(MainGameSceneNumber, LoadSceneMode.Single);
    }

    public void GameMenu()
    {
        Time.timeScale = normalTime;
        Time.fixedDeltaTime = normalFixedTime;
        SceneManager.LoadSceneAsync(GameMenuSceneNumber, LoadSceneMode.Single);
    }

}

