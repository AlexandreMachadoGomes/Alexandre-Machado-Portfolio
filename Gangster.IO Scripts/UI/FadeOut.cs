using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour
{

    public bool readyFadeOut = false;
    public bool isMesh = false;
    public bool isSprite = false;
    public bool isInput = false;

    public float fadeOutTime = 2;

    private float timer = 0;

    public float timeTillFadeout = 2;
    public bool fadeOutWithTimer = false;


    private Color oldColor;
    private MeshRenderer thisMesh;
    private SpriteRenderer thisSprite;
    private InputField thisInput;
    private Text thisText;

    private bool gotColor = false;

    public Text text;

    // Start is called before the first frame update
    void Start()
    {
        if (fadeOutWithTimer)
            Invoke("StartFade", 2);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (readyFadeOut && timer <= fadeOutTime)
            FadingOut();
    }

   private void StartFade()
    {
        readyFadeOut = true;
    }

    private void FadingOut()
    {
        if (!gotColor)
        {
            

            if (isMesh)
            {
                thisMesh = GetComponent<MeshRenderer>();
                oldColor = thisMesh.material.color;
                Color alphaColor = oldColor;
                alphaColor.a = Mathf.Lerp(1, 0, timer / fadeOutTime);
                thisMesh.material.color = alphaColor;
            }
            else if (isInput)
            {
                thisInput = GetComponent<InputField>();
                oldColor = thisInput.colors.normalColor;
                Color oldColorText = text.color;
                Color alphaColor2 = thisInput.colors.highlightedColor;
                Color alphaColor3 = thisInput.colors.pressedColor;
                Color alphaColor4 = thisInput.colors.selectedColor;
                Color alphaColor5 = thisInput.colors.disabledColor;

                Color alphaColor = oldColor;
                float alpha = Mathf.Lerp(1, 0, timer / fadeOutTime); ;
                alphaColor.a = alpha;
                oldColorText.a = alpha;
                alphaColor2.a = alpha;
                alphaColor3.a = alpha;
                alphaColor4.a = alpha;
                alphaColor5.a = alpha;

                ColorBlock colorblock = thisInput.colors;
                colorblock.normalColor = alphaColor;
                colorblock.highlightedColor = alphaColor2;
                colorblock.pressedColor = alphaColor3;
                colorblock.selectedColor = alphaColor4;
                colorblock.disabledColor = alphaColor5;
                text.color = oldColorText;
                thisInput.colors = colorblock;

                

            }
            else if (isSprite)
            {
                thisSprite = GetComponent<SpriteRenderer>();
                oldColor = thisSprite.material.color;
                Color alphaColor = oldColor;
                alphaColor.a = Mathf.Lerp(1, 0, timer / fadeOutTime);
                thisSprite.material.color = alphaColor;
            }
            else
            {
                thisText = GetComponent<Text>();
                oldColor = thisText.color;
                Color alphaColor = oldColor;
                alphaColor.a = Mathf.Lerp(1, 0, timer / fadeOutTime);
                thisText.color = alphaColor;

            }
        }

        

 

        timer += Time.deltaTime;
    }
}
