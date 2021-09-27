using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{

    private Rigidbody thisRigidbody;
    public float speed;
    private float timeSpawn = 0;
    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        MoveBullet2();
        
    }

    // Update is called once per frame
    void Update()
    {


        timeSpawn += Time.deltaTime;
        if (timeSpawn >= 3)
            Destroy(this.gameObject);

        if (thisRigidbody.velocity.magnitude < 2)
        {
            //Destroy(this.gameObject);
        }
    }

    private void MoveBullet()
    {
        thisRigidbody.AddForce(transform.forward * speed, ForceMode.Force);
    }

    private void MoveBullet2()
    {
        thisRigidbody.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Player>().TakeDamage();
            Destroy(this.gameObject);
        }
    }

}
 