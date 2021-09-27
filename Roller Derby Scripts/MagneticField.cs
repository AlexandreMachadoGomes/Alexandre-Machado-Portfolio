using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticField : MonoBehaviour
{
    public Transform player;
    public List<Rigidbody> attractedObjects;
    public float magnetForce = 1;
    public float magnetRange = 1;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SphereCollider>().radius *= magnetRange;
        attractedObjects = new List<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Attract();
        transform.position = player.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        attractedObjects.Add(other.GetComponent<Rigidbody>());
        other.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        attractedObjects.Remove(other.GetComponent<Rigidbody>());
    }

    private void Attract()
    {
        for (int i = 0; i < attractedObjects.Count; i++)
        {
            if (attractedObjects[i] != null)
            {
                Vector3 distance = player.position - attractedObjects[i].position;
                float force = (1 / (distance).magnitude) * magnetForce * 0.4f;
                attractedObjects[i].AddForce(distance * force, ForceMode.Force);
            }
            else
                attractedObjects.Remove(attractedObjects[i]);
        }

    }   

    public void RemoveFromList(Transform target)
    {
        attractedObjects.Remove(target.GetComponent<Rigidbody>());
    }

}
