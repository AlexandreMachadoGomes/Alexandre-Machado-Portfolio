using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;

    public enum tutorialStep
    {
        TAP, 
        DRAG,
        COLLECT,
        RELEASE
    }
    public tutorialStep currentStep;
    public bool hasRotated = false;
    public bool hasCollected = false;
    public bool hasStacked = false;

    private SequencialCanvasManager tutorialCanvas;

    void Start()
    {

        PlayerPrefs.SetFloat("thisRunScore", 0);

        tutorialCanvas = GetComponent<SequencialCanvasManager>();

        currentStep = tutorialStep.TAP;

        if(TutorialManager.Instance == null)
            TutorialManager.Instance = this;
            
    }

    void FixedUpdate()
    {
        
        if(currentStep == tutorialStep.TAP)
        {
            if(Input.GetMouseButton(0))
            {
                // Change state
                currentStep = tutorialStep.DRAG;

                // Configure Canvas
                if(tutorialCanvas)
                    tutorialCanvas.NextCanvas();
            }
        }
        else if(currentStep == tutorialStep.DRAG)
        {
            if(Input.GetMouseButton(0) )
            {
                // Change state
                currentStep = tutorialStep.COLLECT;

                // Configure Canvas
                if(tutorialCanvas)
                    tutorialCanvas.NextCanvas();
            }
        }
        else if(currentStep == tutorialStep.COLLECT)
        {
            if(hasCollected)
            {
                // Change state
                currentStep = tutorialStep.RELEASE;

                // Configure Canvas
                if(tutorialCanvas)
                    tutorialCanvas.NextCanvas();
            }
        }
        else if(currentStep == tutorialStep.RELEASE)
        {
            if(hasStacked || !Input.GetMouseButton(0))
            {
                // Configure Canvas
                if(tutorialCanvas)
                    tutorialCanvas.NextCanvas();

                CratesManager.Instance.ReleaseCrate();

                Invoke("LoadGame", 2);
            }
        }
    }

    private void LoadGame()
    {
        // Load next scene
        SceneManager.LoadScene("Level 1");
    }
}
