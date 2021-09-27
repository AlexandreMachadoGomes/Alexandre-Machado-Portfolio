using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int GameOverSceneNumber;
    public int GameWinSceneNumber;
    public int MainGameSceneNumber;
    public int GameMenuSceneNumber;
    public int GameTutorialSceneNumber;

    public void GameOver()
    {
        SceneManager.LoadSceneAsync(GameOverSceneNumber, LoadSceneMode.Single);
    }

    public void GameWin()
    {
        SceneManager.LoadSceneAsync(GameWinSceneNumber, LoadSceneMode.Single);
    }

    public void StartGame()
    {
        SceneManager.LoadSceneAsync(MainGameSceneNumber, LoadSceneMode.Single);
    }

    public void GameMenu()
    {
        SceneManager.LoadSceneAsync(GameMenuSceneNumber, LoadSceneMode.Single);
    }

    public void GameTutorial()
    {
        SceneManager.LoadSceneAsync(GameTutorialSceneNumber, LoadSceneMode.Single);
    }
}
