using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsHolder : MonoBehaviour
{

    private Transform obsStopper;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        while (0 < transform.childCount)
        {
            Transform child = transform.GetChild(0);
            child.transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = false;
            child.SetParent(null);
        }
    }

}
