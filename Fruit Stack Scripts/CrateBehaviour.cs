using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrateBehaviour : MonoBehaviour
{
    public Transform car;

    private float rotationSpeed;

    public bool notCollidable = false;

    public Text totalPoints;

    public bool gotPoints = false;

    public float maxRotSpeed;
    public float minRotSpeed;
    [Range(1, 20)]
    public float rotationAcceleration;
    public float maxSpeed;
    public bool rotate = true;

    public SliderCounter ActualCounterSlider;

    public float crateDownSpeed = 1;

    private Rigidbody myBody;

    private float lastMouseXPos;

    private float deltaMouseDrag;

    public CratesManager crateManager;

    public float missAngle = 15;
    public float goodMergeAngle = 10;
    public float perfectMergeAngle = 5;

    private float halfCrateHeight = 0.317527f;
    public float cratesMergedCount = 0;

    public bool cantMerge = false;

    public GameObject mergeCrate;

    public bool onGround = false;

    public LifeBar lifeBar;

    public GameObject textPerfect;
    public GameObject textGood;

    public Vector3 textPos;

    public Transform canvas;

    private FruitCounter fruitCounter;

    public bool isMiss = false;
    private bool missFallToRight = false;
    private Vector3 rightMovePos;
    public List<GameObject> colliders;


    public GameObject shockwaveNormal;
    public GameObject shockwaveGood;
    public GameObject shockwavePerfect;

    void Start()
    {
        int direction = Random.Range(0, 2);
        if (direction <= 0)
            direction = -1;

        rotationSpeed = Random.Range(minRotSpeed, maxRotSpeed) * direction;

        myBody = GetComponent<Rigidbody>();

        fruitCounter = transform.GetChild(1).GetChild(0).GetComponent<FruitCounter>();
    }

    void FixedUpdate()
    {
        if (rotate)
        {
            DragToRotate();

            if (deltaMouseDrag != 0)
                rotationSpeed = Mathf.Clamp(rotationSpeed - deltaMouseDrag, -maxSpeed, maxSpeed);

            rotationSpeed *= 0.95f;

            float targetAngle = transform.rotation.eulerAngles.z;
            targetAngle += rotationSpeed / 10;

            Quaternion targetRot = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, targetAngle);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, .8f);

            transform.position = transform.position + Vector3.down * .005f * crateDownSpeed;

        }
        else if (isMiss)
        {
            if (missFallToRight)
            {
                //myBody.AddForce(rightMovePos * -15, ForceMode.Force);
                myBody.AddForceAtPosition(Vector3.up * 35, transform.position + transform.right * .9f, ForceMode.Force);
            }
            else
            {
                //myBody.AddForce(rightMovePos * 15, ForceMode.Force);
                myBody.AddForceAtPosition(Vector3.up * 35, transform.position + transform.right * -.9f, ForceMode.Force);
            }

        }

        if (myBody.velocity.y > 0 && !rotate)
        {
           // myBody.isKinematic = true;
        }

    }


   

    private void OnTriggerEnter(Collider other)
    {
        if (((other.gameObject.CompareTag("crate") && !other.transform.parent.parent.GetComponent<CrateBehaviour>().notCollidable) || other.gameObject.CompareTag("Ground"))  && !onGround)
        {
            ActualCounterSlider.caughtUp = false;
            onGround = true;


            totalPoints.text = "0";
            if (crateManager)
            {
                if (crateManager.cratesList.Count > 1 && !cantMerge)
                {
                    cantMerge = true;
                    CheckForMerge();
                    //crateManager.ReleaseCrate();
                    return;
                }
                else if (rotate)
                {
                    if (!gotPoints)
                    {
                        crateManager.DealPoints(1);
                    }
                    crateManager.ReleaseCrate();

                }
                else
                {
                    if (!gotPoints)
                    {
                        crateManager.DealPoints(1);
                    }
                    if (!cantMerge)
                        CreateShockwave(shockwaveNormal);
                }
                if (cantMerge)
                {
                    //myBody.isKinematic = true;
                }
            }
        }
    }

    public void DragToRotate()
    {
        if (Input.touchCount > 0 || Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMouseXPos = Input.mousePosition.x;
                return;
            }

            deltaMouseDrag = Mathf.Clamp((Input.mousePosition.x - lastMouseXPos), -100, 100);

            //Debug.Log(deltaMouseDrag);

            lastMouseXPos = Input.mousePosition.x;

            if(TutorialManager.Instance)
                TutorialManager.Instance.hasRotated = true;
        }
    }




    public void CheckForMerge()
    {
        
        CrateBehaviour lastCrate = crateManager.cratesList[crateManager.cratesList.Count -2].GetComponent<CrateBehaviour>();
        Quaternion crateRotation = crateManager.cratesList[crateManager.cratesList.Count - 2].transform.rotation;

        //spawns point text 
        

        
        //for the merge to work with both sides of the boxes
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        Quaternion quaternionRotation = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z + 180));

        float angleDistance = Quaternion.Angle(crateRotation, transform.rotation);
        float angleDistance2 = Quaternion.Angle(crateRotation, quaternionRotation);

        if ((angleDistance < perfectMergeAngle || angleDistance2 < perfectMergeAngle) && lastCrate.cratesMergedCount < 2)
        {
            cratesMergedCount += lastCrate.cratesMergedCount + 1;

            Vector3 newPos = lastCrate.transform.position + Vector3.up * ((cratesMergedCount) * halfCrateHeight);
            Quaternion newRot = lastCrate.transform.rotation;

            crateManager.cratesList.Remove(lastCrate.gameObject);
            Destroy(lastCrate.gameObject);

            GameObject newCrate = Instantiate(mergeCrate, newPos, newRot);
            newCrate.transform.parent = car;
            newCrate.transform.localScale = new Vector3(newCrate.transform.localScale.x, newCrate.transform.localScale.y, newCrate.transform.localScale.z * (cratesMergedCount + 1));
            newCrate.GetComponent<Rigidbody>().isKinematic = false;

            crateManager.cratesList.Add(newCrate);

            CrateBehaviour newCrateScript = newCrate.GetComponent<CrateBehaviour>();

            newCrateScript.cratesMergedCount = cratesMergedCount;
            newCrateScript.rotate = false;
            newCrateScript.onGround = false;
            newCrateScript.crateManager = crateManager;
            newCrateScript.canvas = canvas;
            newCrateScript.crateDownSpeed = crateDownSpeed;
            newCrateScript.lifeBar = lifeBar;
            newCrateScript.goodMergeAngle = goodMergeAngle;
            newCrateScript.perfectMergeAngle = perfectMergeAngle;
            newCrateScript.missAngle = missAngle;
            newCrateScript.gotPoints = true;
            crateManager.cratesList.Add(newCrate);

            crateManager.cratesList.Remove(gameObject);

            if (!gotPoints)
            {
                crateManager.DealPoints(2);
            }
            //fruitCounter.AddFruits();

            CreateShockwave(shockwavePerfect);

            Instantiate(textPerfect, textPos, Quaternion.identity, canvas);

            //Handheld.Vibrate();

            Destroy(gameObject);
            return;
        }
        else if ((angleDistance < goodMergeAngle || angleDistance2 < goodMergeAngle) && lastCrate.cratesMergedCount < 2)
        {
            cratesMergedCount += lastCrate.cratesMergedCount + 1;

            Vector3 newPos = lastCrate.transform.position + Vector3.up * halfCrateHeight;
            Quaternion newRot = lastCrate.transform.rotation;

            crateManager.cratesList.Remove(lastCrate.gameObject);
            Destroy(lastCrate.gameObject);

            GameObject newCrate = Instantiate(mergeCrate, newPos, newRot);
            newCrate.transform.parent = car;
            newCrate.transform.localScale = new Vector3(newCrate.transform.localScale.x, newCrate.transform.localScale.y, newCrate.transform.localScale.z * (cratesMergedCount + 1));
            newCrate.GetComponent<Rigidbody>().isKinematic = false;

            crateManager.cratesList.Add(newCrate);

            CrateBehaviour newCrateScript = newCrate.GetComponent<CrateBehaviour>();

            newCrateScript.cratesMergedCount = cratesMergedCount;
            newCrateScript.rotate = false;
            newCrateScript.onGround = true;
            newCrateScript.crateManager = crateManager;
            newCrateScript.canvas = canvas;
            newCrateScript.crateDownSpeed = crateDownSpeed;
            newCrateScript.lifeBar = lifeBar;
            newCrateScript.goodMergeAngle = goodMergeAngle;
            newCrateScript.perfectMergeAngle = perfectMergeAngle;
            newCrateScript.missAngle = missAngle;
            newCrateScript.gotPoints = true;
            crateManager.cratesList.Add(newCrate);

            crateManager.cratesList.Remove(gameObject);

            CreateShockwave(shockwaveGood);

            if (!gotPoints)
            {
                crateManager.DealPoints(1.5f);
            }
            //fruitCounter.AddFruits();

            Instantiate(textGood, textPos, Quaternion.identity, canvas);

            //Handheld.Vibrate();

            Destroy(gameObject);
            return;

        }
        else
        {
            Quaternion quaternionRotation_2 = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z + 90));
            Quaternion quaternionRotation_3 = Quaternion.Euler(new Vector3(eulerAngles.x, eulerAngles.y, eulerAngles.z + 270));
            float angleDistance_Miss = Quaternion.Angle(crateRotation, quaternionRotation_2);
            float angleDistance2_Miss = Quaternion.Angle(crateRotation, quaternionRotation_3);
            cantMerge = true;
            if (angleDistance_Miss < missAngle || angleDistance2_Miss < missAngle)
            {
                
                int randomFallPos = Random.Range(0, 1);
                myBody.isKinematic = false;

                if (randomFallPos > 0)
                    missFallToRight = true;
                else
                    missFallToRight = false;

                gameObject.tag = "Untagged";

                isMiss = true;
                notCollidable = true;
                Invoke("StopFallingForce", .2f);
                
                myBody.constraints = RigidbodyConstraints.None;
                rightMovePos = transform.right;
                crateManager.cratesList.Remove(this.gameObject);
                gameObject.transform.parent = null;
                Camera.main.GetComponent<CameraMovement>().targetTransform = lastCrate.transform;
                crateManager.canSpawnCrate = false;

                fruitCounter.RemoveFromCounter();
                lifeBar.LooseLife();
                return;
            }
            crateManager.DealPoints(1);
        }

        CreateShockwave(shockwaveNormal);

        cantMerge = true;
        //myBody.isKinematic = true;
        if (!gotPoints)
        {
            crateManager.DealPoints(1);
        }
        //fruitCounter.AddFruits();
    }


    private void StopFallingForce()
    {
        crateManager.canSpawnCrate = true;
        //gameObject.layer = 8;
        for (int i = 0; i < colliders.Count; i++)
        {
            colliders[i].layer = 8;
        }
        isMiss = false;
    }

    private void CreateShockwave(GameObject shockwaveSpawn)
    {
        ParticleSystem shockwaveParticle =  Instantiate(shockwaveSpawn, transform.position + Vector3.down * halfCrateHeight, Quaternion.identity, canvas).transform.GetChild(0).GetComponent<ParticleSystem>();

    }

}
