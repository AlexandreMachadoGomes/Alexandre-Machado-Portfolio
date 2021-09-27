using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpAndDown : MonoBehaviour
{

    private float timer = 3;

    public bool goingUp = false;

    public float speed;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpNDown();
    }

    private void UpNDown()
    {
        timer -= Time.deltaTime;

        if (goingUp)
            transform.position = transform.position + Vector3.up * speed;
        else
            transform.position = transform.position + Vector3.down * speed;

        if (timer < 0)
        {
            timer = 3;
            goingUp = !goingUp;
        }
    }

}
