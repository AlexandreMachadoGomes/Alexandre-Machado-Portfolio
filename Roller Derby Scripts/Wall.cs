using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    public float force;
    public ParticleSystem particles;
    public bool righSide;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 rot;
        if (righSide)
            rot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - 90);
        else
            rot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 90);
        ParticleSystem particle = Instantiate(particles, collision.GetContact(0).point, transform.rotation);
        particle.transform.rotation = Quaternion.Euler(rot);
        //collision.gameObject.GetComponent<Rigidbody>().AddForce(collision.GetContact(0).normal.normalized * force, ForceMode.Impulse);
    }

}
