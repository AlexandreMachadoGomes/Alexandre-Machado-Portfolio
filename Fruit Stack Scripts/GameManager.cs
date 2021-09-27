using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public GameObject menuCanvas;
    public GameObject gameCanvas;
    public GameObject tutorialCanvas;
    
    public bool gameStarted = false;

    public int sceneLoadOrder = 1;
    public int levelNumer = 0;

    public FloatVariable points;

    public List<GameObject> lifecrates;


    public GameObject crateManager;
    public GameOverCar carScript;
    public PlayerInput playerInput;
    public CameraMovement camera;

    public FadeInTutorial gameWonText;
    public FadeInButton tryAgainButton;
    public FadeInTutorial gameOverText;

    private void Start()
    {
        
    }


    public void NextLevel()
    {
        Invoke("NextlevelContinued", 2);
        PlayerPrefs.SetFloat("thisRunScore", PlayerPrefs.GetFloat("thisRunScore") + crateManager.GetComponent<CratesManager>().pointsVariable.Value);
        gameWonText.fadeIn = true;

        camera.enabled = false;
        playerInput.gameOver = true;
        crateManager.SetActive(false);
        gameCanvas.SetActive(false);
    }

    private void NextlevelContinued()
    {
        SceneManager.LoadSceneAsync(sceneLoadOrder);
    }

    public void GameOver()
    {
        
        camera.enabled = false;
        playerInput.gameOver = true;
        crateManager.SetActive(false);
        carScript.gameOver = true;
        gameCanvas.SetActive(false);
        gameOverText.fadeIn = true;
      

        PlayerPrefs.DeleteKey("thisRunScore");

        Invoke("GameOverContinued", 2);

        //SceneManager.LoadSceneAsync(0);
    }


    private void GameOverContinued()
    {
        tryAgainButton.StartFadein();
    }

    public void StartGame()
    {
        if(menuCanvas)
            menuCanvas.SetActive(false);
        if(gameCanvas)
            gameCanvas.SetActive(true);

        if(TutorialManager.Instance == null)
        {
            for (int i = 0; i < lifecrates.Count; i++)
            {
                lifecrates[i].active = true;
            }
        }

        tutorialCanvas.SetActive(false);

        gameStarted = true;
    }

    void FixedUpdate()  
    {
        if(Input.GetMouseButton(0) && !gameStarted)
            StartGame();
    }
}
