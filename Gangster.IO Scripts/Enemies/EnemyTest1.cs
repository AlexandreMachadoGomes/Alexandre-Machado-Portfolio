using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTest1 : MonoBehaviour
{
    public int hitPoints;
    public float fireCooldownMax;
    private float cooldown;
    public GameObject Player;
    public bool isSeeingPlayer;
    public GameObject projectile;
    private Rigidbody thisRigidbody;
    public float rotationVelocity;
    private Vector3 rotationDirection = Vector3.down;
    //public GameObject restDir;
    private Vector3 StartingPoint;
    public GameObject movePoint1;
    public GameObject movePoint2;
    private Vector3 movePointPresent;
    public float moveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        StartingPoint = transform.position;
        movePointPresent = movePoint1.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirection();
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            Fire();
        }
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            Destroy(collision.gameObject);

            if (hitPoints < 0)
                Destroy(this.gameObject);

            hitPoints -= 1;
        }
    }


    private void Fire()
    {
        Vector3 projectileSpawnPos = new Vector3(0, 0.3f, 0);
        Instantiate(projectile, transform.position + transform.forward + projectileSpawnPos, transform.rotation);
        cooldown = fireCooldownMax;
    }

    private void MoveDirection()
    {   
        Vector3 moveDir = movePointPresent - transform.position;
        if (moveDir.magnitude < 0.2f)
        {
            if (movePointPresent == movePoint1.transform.position)
                movePointPresent = movePoint2.transform.position;
            else
                movePointPresent = movePoint1.transform.position;
        }
        thisRigidbody.MovePosition(transform.position + moveDir.normalized * moveSpeed * Time.deltaTime);
    }

    /*private void LookAt(GameObject thing)
    {
        float aux = Vector3.Angle(transform.forward, thing.transform.position - transform.position);

        if (aux > 5)
        {

            //Vector3 rotation = new Vector3(0, deltaMousePos * 6, 0);
            //Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);
            float desiredAngle = Quaternion.LookRotation(thing.transform.position - transform.position, transform.up).eulerAngles.y;

            if (Mathf.Abs(transform.eulerAngles.y - desiredAngle) < 180)
            {
                if (desiredAngle > transform.eulerAngles.y)
                    rotationDirection = Vector3.up;
                else
                    rotationDirection = Vector3.down;
            }
            Quaternion deltaRotation = Quaternion.Euler(rotationVelocity * Time.deltaTime * rotationDirection);

            Debug.Log(aux);
            thisRigidbody.MoveRotation(thisRigidbody.rotation * deltaRotation);


        }
    }
    */
}
