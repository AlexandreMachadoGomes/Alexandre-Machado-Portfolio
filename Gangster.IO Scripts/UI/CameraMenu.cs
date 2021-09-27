using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{

    public List<Transform> lookingPoints;

    public Transform presentLookPoint;
    private Quaternion lookingRotation;

    private bool isMoving = false;

    private float timer = 0;
    public float maxTimer = 3;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(0,1, 18);
        transform.LookAt(presentLookPoint);
        //Invoke("SetNewLookingPoint2", 5f);
        //Invoke("SetNewLookingPoint3", 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (timer < maxTimer)
            timer += Time.deltaTime;
        MoveCamera();
    }

    public void SetNewLookingPoint()
    {
        presentLookPoint = lookingPoints[1];
        timer = 0;
        isMoving = true;
        lookingRotation = Quaternion.LookRotation(presentLookPoint.position - transform.position, Vector3.up);
    }

    public void SetNewLookingPoint2()
    {
        presentLookPoint = lookingPoints[0];
        timer = 0;
        isMoving = true;
        lookingRotation = Quaternion.LookRotation(presentLookPoint.position - transform.position, Vector3.up);
    }

    public void SetNewLookingPointInt(int lookingPointNumber)
    {
        presentLookPoint = lookingPoints[lookingPointNumber];
        timer = 0;
        isMoving = true;
        lookingRotation = Quaternion.LookRotation(presentLookPoint.position - transform.position, Vector3.up);
    }

    private void MoveCamera()
    {
        if (isMoving)
        {
            if (timer < maxTimer / 2)
            {
                transform.rotation = Quaternion.Lerp(Quaternion.Lerp(transform.rotation, lookingRotation, .5f), transform.rotation, 1 - (timer * 2 / maxTimer));
            }
            else if (timer < maxTimer && timer > maxTimer/2)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, lookingRotation, timer / maxTimer);
            }
            if (timer > maxTimer)
                isMoving = false;
        }
    }

}
