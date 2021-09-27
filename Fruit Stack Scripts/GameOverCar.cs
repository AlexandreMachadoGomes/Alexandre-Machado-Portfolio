using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCar : MonoBehaviour
{

    public bool gameOver = false;

    public float moveSpeed = .01f;

    private Rigidbody thisRigidBody;

    private float timer = 0;

    public Transform rotatePos;

    private bool removeCrateKinematic = true;
    public CratesManager crateManager;


    // Start is called before the first frame update
    void Start()
    {
        thisRigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gameOver)
            MoveFowards();
    }


    private void MoveFowards()
    {
        if (timer < 3 )
            RotateCar();
        if (moveSpeed < .5f)
            moveSpeed *= 1.015f;
        thisRigidBody.MovePosition(transform.position + transform.forward * moveSpeed);
        timer += Time.deltaTime;
    }

    private void RotateCar()
    {

        for (int i = 2; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
        }

        //Quaternion newRot =  Quaternion.Euler(0, transform.rotation.eulerAngles.y + - moveSpeed  * 7, 0);
        //thisRigidBody.MoveRotation(newRot);
        transform.RotateAround(rotatePos.position, Vector3.down, moveSpeed * 7f);
        
    }
}
