using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallProjectile : MonoBehaviour
{
    public Player player;
    public Transform target;
    private Rigidbody thisRigidbody;
    private Vector3 lastPos;
    private Vector3 direction = Vector3.zero;
    private Vector3 startDistance;
    public MagneticField playerMagneticField;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = this.GetComponent<Rigidbody>();
        startDistance = target.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        MoveProjectile();
    }

    private void MoveProjectile()
    {
        thisRigidbody.AddForce(Vector3.down * 3000);
        if (transform.position != lastPos)
        {
            direction = (transform.position - lastPos).normalized;
            lastPos = transform.position;
        }
        if (target)
        {
            Vector3 distance = target.position - transform.position;
            float distanceAux = Mathf.Clamp((distance.x / startDistance.x), 0, 1) + 0.5f;
            if (distance.x > 0.8f)
                thisRigidbody.AddForce(Vector3.right * distanceAux * 80 * (distance.x / Mathf.Abs(distance.x)), ForceMode.Force);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("damage"))
        {
            Explode();
        }
        else if (other.gameObject.CompareTag("Bot"))
        {
            Explode();
            other.gameObject.GetComponent<Bot>().Explode(other.GetContact(0).normal * -1);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {

            Transform target = other.transform.parent.GetChild(0);
            Rigidbody targetRig = target.GetComponent<Rigidbody>();
            target.GetComponent<Collider>().isTrigger = true;
            targetRig.isKinematic = true;
            target.gameObject.layer = 0;
            //playerMagneticField.RemoveFromList(target);
            targetRig.detectCollisions = false;
            other.transform.parent.SetParent(transform);
        }
        else if (other.gameObject.CompareTag("clickPhase"))
        {
            Explode();
        }
    }


    private void Explode()
    {
        if (direction == Vector3.zero)
            direction = Vector3.back;
        while ( 0 < transform.childCount)
        {
            Transform target = transform.GetChild(0).GetChild(0);
            Vector3 explosionDir = transform.GetChild(0).transform.position - transform.position;
            if (explosionDir.z < 0.10f)
                explosionDir.z = 0.10f;
            Rigidbody targetRig = target.GetComponent<Rigidbody>();
            Collider targetCol = target.GetComponent<Collider>();
            target.gameObject.layer = 18;
            targetRig.isKinematic = false;
            targetRig.AddForce(explosionDir.normalized + thisRigidbody.velocity * -1.2f, ForceMode.Impulse);
            targetRig.detectCollisions = true;
            targetCol.material = player.bouncyMaterial;
            targetCol.isTrigger = false;
            transform.GetChild(0).parent = null;
        }
        this.GetComponent<MeshRenderer>().enabled = false;
        thisRigidbody.isKinematic = true;
        Destroy(gameObject);
    }

}
