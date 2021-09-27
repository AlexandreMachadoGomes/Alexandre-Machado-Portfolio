using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreenTarget : MonoBehaviour
{
    public bool milestoneAcomplished = false;

    public Transform rotatePos;
    private float angle = 0;
    private bool shotEffect = false;

    public ParticleSystem shotParticleEffect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (milestoneAcomplished && angle < 95)
            MoveDown();
    }

    private void MoveDown()
    {

        if (!shotEffect)
        {
            shotEffect = true;
            shotParticleEffect.Play();
        }
        transform.RotateAround(rotatePos.position, transform.right, -4);
        angle += 4;
    }
}
