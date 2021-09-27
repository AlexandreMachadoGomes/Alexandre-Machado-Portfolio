using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderCounter : MonoBehaviour
{

    public Slider preCounterSlider;
    private Slider thisSlider;
    public Slider mainSlider;
    public bool caughtUp = true;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        thisSlider = GetComponent<Slider>();
        thisSlider.maxValue = preCounterSlider.maxValue * 100;
    }

    // Update is called once per frame
    void Update()
    {
        if (!caughtUp)
            CatchUpOnPreCounterSlider();
    }


    private void CatchUpOnPreCounterSlider()
    {
        thisSlider.value += 20;
        if (thisSlider.value / 100 > preCounterSlider.value)
            caughtUp = true;
        if (mainSlider.value >= mainSlider.maxValue)
            gameManager.NextLevel();
    }
}
