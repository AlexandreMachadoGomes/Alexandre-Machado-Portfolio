using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextEffect : MonoBehaviour
{

    public float timeTillDelete = 2;
    private float startTime = 0;

    public float upSpeed =  1;

    private Text pointText;

    // Start is called before the first frame update
    void Start()
    {
        pointText.CrossFadeAlpha(0, 2, false);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        transform.position += Vector3.up * upSpeed/100;
        startTime += Time.deltaTime;
    }
}
