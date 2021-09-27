using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberShrink : MonoBehaviour
{

    public bool isShrinking = false;

    private Vector3 initialScale;

    // Start is called before the first frame update
    private void Start()
    {
        initialScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isShrinking)
        {
            Shrink();
        }
    }

    private void Shrink()
    {
        transform.localScale *= 0.95f;
        if (transform.localScale.magnitude <= initialScale.magnitude)
        {
            isShrinking = false;
        }
    }

    public void PickedFruit()
    {
        transform.localScale = initialScale * 1.5f;
        isShrinking = true;
    }

}
