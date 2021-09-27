using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform targetTransform;

    private float Y_offset;


    [Range(0, 1)]
    public float damp;

    private void Start()
    {
        if (targetTransform)
            Y_offset = this.transform.position.y - targetTransform.position.y - 1;
        else
            Y_offset = this.transform.position.y;
    }

    void FixedUpdate()
    {
        if (targetTransform)
        {
            Vector3 newPosition = this.transform.position;
            newPosition.y = Y_offset + targetTransform.position.y ;

            transform.position = Vector3.Lerp(transform.position, newPosition, 1-damp);
        }
    }
}
