using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightControlGameOver : MonoBehaviour
{

    private Light spotlight;

    // Start is called before the first frame update
    void Start()
    {
        spotlight = GetComponent<Light>();
        Invoke("Activate", .5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Activate()
    {
        spotlight.intensity = 2;
    }
}
