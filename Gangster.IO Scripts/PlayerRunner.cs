using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunner : MonoBehaviour
{

    private bool notSwiping = true;
    private Rigidbody thisRigidbody;
    private Vector3 lastMousePos;
    public GameObject projectile;
    public float moveSpeed;
    public float moveSpeedSide;
    private float lastRotation;
    public int life;
    public float ShotCooldownMax;
    private float actualShotCooldown;


    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = this.GetComponent<Rigidbody>();
        actualShotCooldown = ShotCooldownMax;
    }

    void Update()
    {
        //code for the cellphone version

        

        actualShotCooldown -= Time.deltaTime;


        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
                Fire();
            if (touch.phase == TouchPhase.Began)
                lastMousePos = Input.mousePosition;
            float deltaMousePos = lastMousePos.x - Input.mousePosition.x;
            Vector3 slideAdicionalPos = Vector3.forward * moveSpeedSide * deltaMousePos / 20;
            thisRigidbody.MovePosition(thisRigidbody.position + transform.forward * moveSpeed * Time.deltaTime + slideAdicionalPos);
            lastMousePos = Input.mousePosition;
            thisRigidbody.angularVelocity = Vector3.zero;
        }
        else
            MovePlayer();

        /*
        if (Input.GetMouseButtonUp(0) && actualShotCooldown <= 0)
        {
            Fire();
        }

        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
                lastMousePos = Input.mousePosition;
            float deltaMousePos = lastMousePos.x - Input.mousePosition.x;
            Vector3 slideAdicionalPos = Vector3.forward * moveSpeedSide * deltaMousePos / 20;
            thisRigidbody.MovePosition(thisRigidbody.position + transform.forward * moveSpeed * Time.deltaTime + slideAdicionalPos);
            lastMousePos = Input.mousePosition;
            thisRigidbody.angularVelocity = Vector3.zero;
        }
        else
            MovePlayer();
        */

    }



        private void Fire()
    {
        Vector3 projectileSpawnPos = new Vector3(0, 0.3f, 0);
        Instantiate(projectile, transform.position + transform.forward + projectileSpawnPos, transform.rotation);
        actualShotCooldown = ShotCooldownMax;
    }

    private void MovePlayer()
    {

        thisRigidbody.MovePosition(transform.position + transform.forward * moveSpeed * Time.deltaTime);
    }



}
