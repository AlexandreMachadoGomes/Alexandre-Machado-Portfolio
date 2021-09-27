using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInButton : MonoBehaviour
{

    public string nextScene;
    private bool fadeIn = false;
    private float timer = 0;
    private Image image;
    public float fadeinTime = 2;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeIn)
        {
            timer += Time.deltaTime;
            Color newColor = image.color;
            Color newTextColor = text.color;
            float alpha = Mathf.Clamp(timer / fadeinTime, 0, 1);
            newColor.a = alpha;
            newTextColor.a = alpha;
            image.color = newColor;
            text.color = newTextColor;
            if (timer >= fadeinTime)
            {
                fadeIn = false;
                GetComponent<Button>().interactable = true;
            }
        }
    }

   

    public void StartFadein()
    {
        fadeIn = true;
    }


    public void LoadNextScene()
    {
        SceneManager.LoadSceneAsync(nextScene);
    }

}
