using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraColorChange : MonoBehaviour
{

    public List<Color> colors;

    private int nextLerpColorNumber;
    private int thisColorNumber;

    public float lerpTime = 5;

    private Color actualColor;

    private float timer = 0;

    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        thisColorNumber = 0;
        actualColor = colors[0];
        nextLerpColorNumber = Random.Range(1, colors.Count - 1);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeColor();
    }


    private void ChangeColor()
    {
        timer += Time.deltaTime;
        actualColor = Color.Lerp(colors[thisColorNumber], colors[nextLerpColorNumber], Mathf.Clamp((timer / lerpTime), 0, 1));
        if (timer >= lerpTime)
        {
            timer = 0;
            thisColorNumber = nextLerpColorNumber;
            nextLerpColorNumber = Random.Range(0, colors.Count);
            while (nextLerpColorNumber == thisColorNumber)
                nextLerpColorNumber = Random.Range(0, colors.Count);
        }
        text.color = actualColor;
    }

}
