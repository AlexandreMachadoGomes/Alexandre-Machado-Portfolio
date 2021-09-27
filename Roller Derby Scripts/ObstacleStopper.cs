using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleStopper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StopObjects()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = true;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        other.transform.SetParent(null);
    }
}
