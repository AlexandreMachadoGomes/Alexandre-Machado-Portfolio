using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInTypeName : MonoBehaviour
{
    public float fadeinTime = 1;
    private float timer = 0;
    public bool fadeIn = false;
    public bool fadeOut = false;
    private bool readyToStart = false;
    private bool tutorialStarted = false;



    public List<FadeInTutorial> texts;


    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if (readyToStart)
        {
            if (Input.GetMouseButtonUp(0))
            {
                readyToStart = false;
                for (int i = 0; i < texts.Count; i++)
                {
                    texts[i].fadeOut = true;
                }
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (fadeIn)
            Fade_In();
        else if (fadeOut)
            FadeOut();
    }

    private void Fade_In()
    {
        timer += Time.deltaTime;
        Color newColor = text.color;
        newColor.a = timer / fadeinTime;
        text.color = newColor;

        if (timer > fadeinTime)
        {
            timer = 0;
            fadeIn = false;
            readyToStart = true;
        }

    }

    private void FadeOut()
    {
        timer += Time.deltaTime;
        Color newColor = text.color;
        newColor.a = 1 - timer / fadeinTime;
        text.color = newColor;

        if (timer > fadeinTime)
        {
            timer = 0;
            fadeOut = false;



        }



    }
}
