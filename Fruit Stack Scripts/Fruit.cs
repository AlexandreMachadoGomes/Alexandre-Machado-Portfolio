using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fruit : MonoBehaviour
{

    private Rigidbody thisRB;
    private Vector3 startPos;

    public int points = 5;

    public Transform target;


    private Vector3 finalPos;

    public float time = 0;

    public float hitRandomOffset = 0.3f;

    public bool hitSomething = false;

    public bool caught = false;

    public bool orangeCount = false;

    public bool spawnedParticles = false;

    public CratesManager crateManager;

    public GameObject canvas;


    


    //parabola script variables
    //

    [SerializeField] private Transform TargetObjectTF;
    public float LaunchAngle = 25;





    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        thisRB = GetComponent<Rigidbody>();
        spawnedParticles = false;

        float randomXOffset = Random.Range(.2f, hitRandomOffset);
        float randomZOffset = Random.Range(.2f, hitRandomOffset);
        float randomX = Random.Range(0, 2);
        float randomY = Random.Range(0, 2);
        if (randomX == 0)
            randomXOffset *= -1;
        if (randomY == 0)
            randomXOffset *= -1;
        finalPos = new Vector3(target.position.x + randomXOffset, target.position.y + .5f, target.position.z + randomZOffset);
         
        //parabola script start stuff

        TargetObjectTF = target;
        Launch();


    }



    private void OnCollisionEnter(Collision collision)
    {
        hitSomething = true;

        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 5f);
        }
        
    }



    //parabola script

    void Launch()
    {

        // SetNewTarget();
        // think of it as top-down view of vectors: 
        //   we don't care about the y-component(height) of the initial and target position.
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        Vector3 targetXZPos = new Vector3(finalPos.x, 0, finalPos.z);

        // rotate the object to face the target
        transform.LookAt(finalPos);

        // shorthands for the formula
        float R = Vector3.Distance(projectileXZPos, targetXZPos);
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(LaunchAngle * Mathf.Deg2Rad);
        float H = (finalPos.y + GetPlatformOffset()) - transform.position.y;

        // calculate the local space components of the velocity 
        // required to land the projectile on the target object 
        float Vz = Mathf.Sqrt(G * R * R / (2.0f * (H - R * tanAlpha)));
        
        float Vy = tanAlpha * Vz;

        // create the velocity vector in local space and get it in global space
        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);
        //globalVelocity *= R / 5.5f;
        // launch the object by setting its initial velocity and flipping its state
        thisRB.velocity = globalVelocity;
    }

    // Sets a random target around the object based on the TargetRadius


    float GetPlatformOffset()
    {
        float platformOffset = 0.0f;
        // 
        //          (SIDE VIEW OF THE PLATFORM)
        //
        //                   +------------------------- Mark (Sprite)
        //                   v
        //                  ___                                          -+-
        //    +-------------   ------------+         <- Platform (Cube)   |  platformOffset
        // ---|--------------X-------------|-----    <- TargetObject     -+-
        //    +----------------------------+
        //

        // we're iterating through Mark (Sprite) and Platform (Cube) Transforms. 
        foreach (Transform childTransform in TargetObjectTF.GetComponentsInChildren<Transform>())
        {
            // take into account the y-offset of the Mark gameobject, which essentially
            // is (y-offset + y-scale/2) of the Platform as we've set earlier through the editor.
            if (childTransform.name == "Mark")
            {
                platformOffset = childTransform.localPosition.y;
                break;
            }
        }
        return platformOffset;
    }





}
