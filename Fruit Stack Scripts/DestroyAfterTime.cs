using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float timeTilSelfDestruct = 2;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timeTilSelfDestruct);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
