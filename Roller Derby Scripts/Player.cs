using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float shootCooldownInSeconds = 1;
    public float rotateSpeed = 1;
    public float kickForceIncrementOnClick = 1;
    public float kickForceDecrement = 1;
    public float playerSpeed = 1;
    private Rigidbody thisRigidbody;
    private Vector2 touchInitialPoint;
    private Vector3 mouseInitialPoint;
    private Vector3 lastPos;
    public SmoothFollow camera;
    public float moveSpeed;
    public float maxVelocity;
    private float speed;
    public float explosionForce;
    public Vector3 jumpForce;
    public bool ballDestroyedOnHittingGround = false;
    public bool isJumping = false;
    private Vector3 velocityForJump;
    public PhysicMaterial bouncyMaterial;
    private Touch touch;
    public int amountCubesLost;
    public bool clickingPhase = false;
    private float clickPhaseCD = 0;
    private float clickPower = 1;
    public MagneticField magneticField;
    public Slider slider;
    public Image sliderColor;
    public Transform scoreHolder;
    private Color color1 = new Color(140, 192, 5, 255);
    private Color color2 = new Color(140, 192, 5, 255);
    public Animator guyAnim;
    public bool startAnim = false;
    public float startAnimTime = 0;
    public ObstacleStopper obsStopper;
    private bool aux = false;
    public List<Transform> enemyTargetList;
    private bool canShoot = true;
    public GameObject ballProj;
    public Animator camAnim;
    private float thisVel;
    private float deltaVel;
    private float rotationValue = 0;
    private Vector3 offset;
    private Vector3 notMovingOffset;
    private float angle;
    private float previousVel;

    public int totalScore = 0;
    public string nextSceneName;


    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        speed = moveSpeed;
        enemyTargetList = new List<Transform>();
        offset = new Vector3(0, -0.35f, -1.2f);
        notMovingOffset = offset;
        deltaVel = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        previousVel = thisVel;
        thisVel = thisRigidbody.velocity.x;
        if (thisVel == 0)
            thisVel = 0.01f;

        


        RotateGuy();





        if (clickingPhase)
        {
            thisRigidbody.AddForce(Vector3.down * 400 * playerSpeed);

            clickPower -= Time.deltaTime * kickForceDecrement;

            if (startAnim)  
            {
                startAnimTime += Time.deltaTime;
                if (startAnimTime > 1)
                    Jump();
                else
                    guyAnim.SetFloat("Blend", startAnimTime);
            }






            
            if (Input.touchCount > 0)
            {

                if (Input.GetTouch(0).phase == TouchPhase.Began && clickPhaseCD > 0.2f)
                {
                    clickPhaseCD = 0;
                    clickPower += 0.05f * kickForceIncrementOnClick;
                    Debug.Log("hit");
                    clickPhaseCD += Time.deltaTime;
                }
            }
            else
                clickPhaseCD += Time.deltaTime;
            


            if (Input.GetMouseButtonDown(0) && clickPhaseCD > 0.1f)
            {
                clickPhaseCD = 0;
                clickPower += 0.05f * kickForceIncrementOnClick;
                Debug.Log("hit");
                clickPhaseCD += Time.deltaTime;
            }
            else
                clickPhaseCD += Time.deltaTime;
                
            if (clickPower < 1)
                clickPower = 1;
            if (clickPower > 1.5f)
            {
                clickPower = 1.5f;
            }

            slider.gameObject.SetActive(true);
            slider.value = (clickPower - 1) * 2;
            float rColor2 = (clickPower - 1) * 2;
            //sliderColor.color = Color.Lerp(color2, color1, rColor2);

        }
        else if (!isJumping)
        {

            thisRigidbody.AddForce(Vector3.down * 400 * playerSpeed);

            if (Input.touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {

                    mouseInitialPoint = Input.mousePosition;
                }
                else
                    MoveBall();

            }

            if (Input.GetMouseButton(0))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    mouseInitialPoint = Input.mousePosition;
                }
                else
                    MoveBall();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ShootBall();
            }
        }

    }

    private void MoveBall()
    {
        float deltaMousePos = mouseInitialPoint.x - Input.mousePosition.x;

        if (Mathf.Abs(deltaMousePos) > 300)
        {
            deltaMousePos /= Mathf.Abs(deltaMousePos);
            deltaMousePos *= 300;
        }
        thisRigidbody.AddForce(Vector3.left * deltaMousePos * speed / 10, ForceMode.Force);

        

    }



    private void OnCollisionEnter(Collision collision)
    {

        if (isJumping)
        {
            Taptic.Warning();
            Explode();
            camAnim.Play("cs", -1, 0.0f);
        }
        else if (!isJumping && collision.collider.CompareTag("damage"))
        {
            Taptic.Heavy();
            LooseCubes();
            camAnim.Play("cs", -1, 0.0f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Taptic.Light();
            Transform target = other.transform.parent.GetChild(0);
            Rigidbody targetRig = target.GetComponent<Rigidbody>();
            target.GetComponent<Collider>().isTrigger = true;
            targetRig.isKinematic = true;
            target.gameObject.layer = 0;
            magneticField.attractedObjects.Remove(target.GetComponent<Rigidbody>());
            //magneticField.RemoveFromList(other.transform);
            targetRig.detectCollisions = false;
            Destroy(other.gameObject);
            other.transform.parent.SetParent(transform);
        }
    }

     
    private void Explode()
    {
        for (int i = 0; i < transform.childCount;)
        {
            Taptic.Warning();
            Transform target = transform.GetChild(0).GetChild(0);
            Rigidbody targetRig = target.GetComponent<Rigidbody>();
            Collider targetCol = target.GetComponent<Collider>();
            Vector3 explosionDir = target.transform.position - transform.position;
            explosionDir.y *= 2.6f;
            if (explosionDir.z < 0.20f)
                explosionDir.z = 0.20f;
            explosionDir.z *= 30;
            target.gameObject.layer = 9;
            targetRig.isKinematic = false;
            targetRig.AddForce(explosionDir.normalized * explosionForce * 1f  * 1.33f, ForceMode.Impulse);
            targetRig.detectCollisions = true;
            targetCol.material = bouncyMaterial;
            targetCol.isTrigger = false;
            target.localScale *= 1.5f;
            target.parent.parent = null;
            target.parent = scoreHolder;

            aux = true;
            
        }

        isJumping = false;
        
        if (ballDestroyedOnHittingGround)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            magneticField.enabled = false;
            gameObject.layer = 13;
            camera.SetMoneyShot();
            scoreHolder.GetComponent<ScoreHolder>().StartScore();
            camAnim.Play("cs", -1, 0.0f);
            Invoke("LoaaNextScene", 8);
        }

    }

    public void Jump()
    {
        this.thisRigidbody.drag = 0;

        

        clickingPhase = false;
        Destroy(slider.gameObject);
        
        magneticField.transform.GetChild(0).gameObject.SetActive(false);
        guyAnim.gameObject.SetActive(false);
        SlowMo();

        Debug.Log("huh");

        isJumping = true;
        thisRigidbody.velocity = new Vector3(0, thisRigidbody.velocity.y, thisRigidbody.velocity.z);
        thisRigidbody.AddForce(jumpForce * 1.3f * clickPower, ForceMode.Impulse);
        GetComponent<SphereCollider>().radius = 0.1f;
        RenderSettings.fogDensity = 0.005f;
        StartCoroutine(jumped());
        
    }


    private void LooseCubes()
    {
        for (int i = 0; i < amountCubesLost; i++)
        {
            if (transform.childCount > 0)
            {
                int cubeIndex = Random.Range(0, transform.childCount - 1);
                Vector3 explosionDir = transform.GetChild(cubeIndex).transform.position - transform.position;
                if (explosionDir.y < 0.10f)
                    explosionDir.y = 0.10f;
                Transform target = transform.GetChild(cubeIndex).GetChild(0);
                magneticField.attractedObjects.Remove(target.GetComponent<Rigidbody>());
                Rigidbody targetRig = target.GetComponent<Rigidbody>();
                Collider targetCol = target.GetComponent<Collider>();
                targetRig.isKinematic = false;
                targetRig.detectCollisions = true;
                targetRig.AddForce(explosionDir.normalized * 6, ForceMode.Impulse);
                targetCol.material = bouncyMaterial;
                target.gameObject.layer = 14;
                targetCol.isTrigger = false;
                target.parent.parent = null;

            }
        }
    }

    private IEnumerator jumped()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SphereCollider>().radius = 1;
        gameObject.layer = 10;
    }

    public IEnumerator KickAnimStartTimer()
    {
        yield return new WaitForSeconds(3);
        startAnim = true;
        guyAnim.SetBool("startKick", true);
    }


    private void SlowMo()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }


    private void ShootBall()
    {
        if (enemyTargetList.Count > 0 && canShoot)
        {
            Transform target = enemyTargetList[0];
            Bot targetScript = target.GetComponent<Bot>();
            targetScript.guyAnim.transform.GetChild(1).GetComponent<Outline>().enabled = false;
            for (int i = 1; i < enemyTargetList.Count; i++)
            {
                targetScript = enemyTargetList[i].GetComponent<Bot>();
                if ((transform.position - enemyTargetList[i].position).magnitude < (transform.position - target.position).magnitude && targetScript)
                    target = enemyTargetList[i];
            }
            GameObject ball = Instantiate(transform.gameObject, transform.position, transform.rotation);
            CleanBall();
            ball.layer = 24;
            Destroy(ball.GetComponent<Player>());
            BallProjectile ballScript = ball.AddComponent<BallProjectile>();
            target.GetComponent<Bot>().followingBalls.Add(ballScript);
            ballScript.gameObject.tag = "PlayerBall";
            ballScript.target = target;
            ballScript.playerMagneticField = magneticField;
            ballScript.player = GetComponent<Player>();
            ball.GetComponent<Rigidbody>().useGravity = false;
            ball.GetComponent<Rigidbody>().drag = 0.3f;
            canShoot = false;
            StartCoroutine(ShootCD());
        }
    }
    
    private IEnumerator ShootCD()
    {
        yield return new WaitForSeconds(shootCooldownInSeconds);
        canShoot = true;
        for (int i = 0; i < enemyTargetList.Count; i++)
        {
            enemyTargetList[i].GetComponent<Bot>().guyAnim.transform.GetChild(1).GetComponent<Outline>().enabled = true;
        }
    }

    private void CleanBall()
    {
        int obsAmount = transform.childCount;
        for (int i = 0; i < obsAmount;i ++)
        {
            Transform child = transform.GetChild(0);
            magneticField.attractedObjects.Remove(child.GetComponent<Rigidbody>());
            child.parent = null;
            Destroy(child.gameObject);

        }
    }


    private void RotateGuy()
    {
        Transform guyTransform = guyAnim.transform;

        if (Mathf.Abs(thisVel) > maxVelocity * 3)
            thisRigidbody.velocity = new Vector3((thisVel / Mathf.Abs(thisVel)) * 3 * maxVelocity, thisRigidbody.velocity.y, thisRigidbody.velocity.z);


        deltaVel = thisVel - previousVel;


        guyTransform.position = transform.position + notMovingOffset;
        guyTransform.RotateAround(transform.position, Vector3.up, deltaVel * 8 * rotateSpeed);
        notMovingOffset = guyTransform.position - transform.position;

    }

    private void LoaaNextScene()
    {
        PlayerPrefs.SetInt("thisRunScore", totalScore);
        SceneManager.LoadSceneAsync(nextSceneName);
    }
}
