using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStroll : MonoBehaviour
{

    public Transform movePoint;

    private Vector3 startPos;

    public int strollMaxTime = 30;
    private float strollTime = 0;

    private float colorChangeTimer = 0;
    public float colorChangeTimerMax = 3;

    private bool StartColorChange1 = false;
    private bool StartColorChange2 = false;

    public MeshRenderer fadePlane;

    private Color oldColor;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        oldColor = fadePlane.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
        strollTime += Time.deltaTime;
        if (StartColorChange1)
            ColorChange();
        else if (StartColorChange2)
            ColorChange2();
    }

    private void MoveCamera()
    {
        transform.position = Vector3.Lerp(startPos, movePoint.position, strollTime / strollMaxTime);
        if (strollMaxTime - strollTime <= colorChangeTimerMax && !StartColorChange1) 
        {
            StartColorChange1 = true;
            colorChangeTimer = 0;
        }
        if (strollTime >= strollMaxTime)
            strollTime = 0;
    }

    private void ColorChange()
    {
        if (colorChangeTimer > colorChangeTimerMax)
        {
            StartColorChange1 = false;
            StartColorChange2 = true;
            colorChangeTimer = 0;
        }

        Color alphaColor = oldColor;
        alphaColor.a = Mathf.Lerp(0, 1, colorChangeTimer / colorChangeTimerMax);
        fadePlane.material.color = alphaColor;

        colorChangeTimer += Time.deltaTime;
    }

    private void ColorChange2()
    {
        if (colorChangeTimer > colorChangeTimerMax)
        {
            StartColorChange2 = false;
        }

        Color alphaColor = oldColor;
        alphaColor.a = Mathf.Lerp(1, 0, colorChangeTimer / colorChangeTimerMax);
        fadePlane.material.color = alphaColor;

        colorChangeTimer += Time.deltaTime;
    }



}
