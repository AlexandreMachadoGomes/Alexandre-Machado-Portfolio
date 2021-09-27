using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
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
    public GameObject restDir;
    public float moveForwardSpeed;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        MoveForwards();

        if (isSeeingPlayer)
        {
            LookAt(Player);

        }
        else
            LookAt(restDir);

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

    private void LookAt(GameObject thing)
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
        else if (cooldown <= 0)
        {
            Fire();
        }
    }

    private void MoveForwards()
    {

        thisRigidbody.MovePosition(transform.position + transform.forward * moveForwardSpeed * Time.deltaTime);
    }

    private void RotateAround(Vector3 direction)
    {
        Quaternion rotationSpeed = Quaternion.Euler(direction * rotationVelocity * Time.deltaTime);
        thisRigidbody.MoveRotation(thisRigidbody.rotation * rotationSpeed);
    }



}
