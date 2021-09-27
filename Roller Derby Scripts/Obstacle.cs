using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).GetComponent<Rigidbody>().isKinematic = true;
        //transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
