using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMoneyShot : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnim()
    {
        GetComponent<MeshRenderer>().material.color = Color.yellow;
    }
}
