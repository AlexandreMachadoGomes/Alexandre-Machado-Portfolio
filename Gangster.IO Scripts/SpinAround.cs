using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAround : MonoBehaviour
{

    public float rotateSpeed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Spin();
    }


    private void Spin()
    {
        transform.Rotate(Vector3.forward * rotateSpeed);
    }
}
