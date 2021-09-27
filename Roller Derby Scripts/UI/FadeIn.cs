using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public float fadeinTime = 1;
    private float timer = 0;
    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool readyToStart = false;
    private bool tutorialStarted = false;


    public List<FadeIn> titleTexts;

    public List<FadeInTutorial> tutorialTexts;

    public bool infiniteFade = true;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeIn)
        {
            Fade_In();
        }
        else if (fadeOut)
            FadeOut();
        

    }

    private void Update()
    {
        if (readyToStart)
        {
            if (Input.GetMouseButtonUp(0))
            {

                titleTexts[0].timer = 0;
                titleTexts[0].fadeOut = true;
                //titleTexts[1].gameObject.SetActive(false);
                infiniteFade = false;
                fadeOut = true;
                fadeIn = false;
                tutorialStarted = true;
                readyToStart = false;
                //startTutorial();
            }
        }
    }

    private void Fade_In()
    {
        timer += Time.deltaTime;
        Color newColor = text.color;
        newColor.a = timer / fadeinTime;
        text.color = newColor;

       if (timer > fadeinTime && infiniteFade)
        {
            timer = 0;
            fadeIn = false;
            fadeOut = true;
            readyToStart = true;
        }

    }

    private void FadeOut()
    {
        timer += Time.deltaTime;
        Color newColor = text.color;
        newColor.a = 1 - timer / fadeinTime;
        text.color = newColor;

        if (timer > fadeinTime && infiniteFade)
        {
            timer = 0;
            fadeIn = true;
            fadeOut = false;
            
        }
        if (tutorialStarted && timer > fadeinTime)
        {
            for (int i = 0; i < tutorialTexts.Count; i++)
            {
                tutorialTexts[i].fadeIn = true;
            }
            tutorialStarted = false;
        }
    }


    public void StartFadeIn()
    {
        fadeIn = true;
    }
}
