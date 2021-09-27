using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float rotSpeed;
    private bool notSwiping = true;
    private Rigidbody thisRigidbody;
    public GameObject projectile;
    public float moveSpeed;
    private float lastRotation;
    public int life;
    public float shotCooldownMax;
    private float actualShotCooldown = 0;
    public bool cellBuild;
    private LineRenderer lineRenderer;
    public Transform body;
    public Transform sight;
    private int sightBounceCount = 0;
    public float sightMaxLenght;
    private float sightLenght;
    public ParticleSystem shootPart;
    public float moveSidewaysSpeed;
    private float initialMousePos;
    private float lastMousePos;

    public GameManager2 gameManager;

    public List<GameObject> aliveEnemies;
    private float score = 0;
    public float levelTime = 60;
    private float actualLevelTime;
    private float levelMaxTime;

    public bool gameOverScreen = false;
    public ParticleSystem bloodAnim;
    public List<GameObject> vidas;
    public bool laserActivated = true;
    public Coroutine laserCDCoroutine;
    public FadeInButton tryAgainButton;
    public FadeInTutorial gameOverText;

    public Text timer1;
    public Text timer2;

    public Slider timerSlider;
    public Image sliderImage;

    public Color green;
    public Color red;

    // Start is called before the first frame update
    void Start()
    {
        if (gameOverScreen)
        {
            Invoke("StartDeath", 1);
        }
        else
            Invoke("DownLevelTimer", 1);
        thisRigidbody = this.GetComponent<Rigidbody>();
        lastRotation = transform.eulerAngles.y;
        lineRenderer = GetComponent<LineRenderer>();
        sightLenght = sightMaxLenght;
        actualLevelTime = levelTime;
        levelMaxTime = levelTime;
    }

    void FixedUpdate()
    {
        //code for the cellphone version

        if (!gameOverScreen)
        {


            actualLevelTime -= Time.deltaTime;
            timerSlider.value = Mathf.Lerp(0, 1, actualLevelTime / levelMaxTime);


            Color newColor = sliderImage.color;

            //newColor = Color.Lerp(red, green, timerSlider.value);

            newColor = Color.HSVToRGB(timerSlider.value * 100f/255f, 87f / 100f, 77f / 100f);
            
            sliderImage.color = newColor;

            MovePlayer();

            if (cellBuild)
                RotateTouch();
            else
                RotatePC();

            if (laserActivated)
                Sight();
                


            actualShotCooldown -= Time.deltaTime;

        }

    }

    private void DownLevelTimer()
    {
        levelTime -= 1;
        timer1.text = levelTime.ToString();
        timer2.text = levelTime.ToString();
        if (levelTime <= 0)
        {
            GameOver();
        }
        Invoke("DownLevelTimer", 1);
    }

    private void Update()
    {
        
        if (Input.GetMouseButtonUp(0) && !gameOverScreen)
        {

            if (actualShotCooldown <= 0)
            {
                if (laserCDCoroutine != null)
                {
                    StopCoroutine(laserCDCoroutine);
                    laserCDCoroutine = null;
                }
                Fire();
            }
            else
            {
                if (laserCDCoroutine == null)
                    laserCDCoroutine = StartCoroutine(CooldownIndicatorOff());

            }
        }
    }

 

    private IEnumerator CooldownIndicatorOff()
    {
        laserActivated = false;
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(.08f);
        laserActivated = true;
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(.08f);
        laserActivated = false;
        lineRenderer.enabled = false;
        yield return new WaitForSeconds(.08f);
        laserActivated = true;
        lineRenderer.enabled = true;
        yield return new WaitForSeconds(.5f);
        laserCDCoroutine = null;
    }


    //code for the cellphone version

    /*private void RotatePlayer(Touch touch)
    {
        float touchDeltaSwipe = lastMousePos.x - touch.position.x;
        Vector3 rotation = new Vector3(0, touchDeltaSwipe, 0);
        Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);
        thisRigidbody.MoveRotation(thisRigidbody.rotation * rotationSpeed);
    }*/

    private void Fire()
    {
        shootPart.Emit(10);
        Vector3 projectileSpawnPos = new Vector3(0, 1.5f, -0.4f);
        Instantiate(projectile, sight.position, transform.rotation);
        actualShotCooldown = shotCooldownMax;
    }

    private void MovePlayer()
    {

        thisRigidbody.MovePosition(transform.position + body.forward * moveSpeed * Time.deltaTime);
    }


    private void RotateTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

                initialMousePos = touch.position.x;
                lastMousePos = initialMousePos;
            }
            if (notSwiping)
            {
                notSwiping = false;
            }

            float delta = lastMousePos - touch.position.x;

            Debug.Log(delta);

            Vector3 rotation = new Vector3(0, delta * rotSpeed, 0);
            Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);
            lastMousePos = touch.position.x;
            thisRigidbody.MoveRotation(thisRigidbody.rotation * rotationSpeed);
            thisRigidbody.angularVelocity = Vector3.zero;
            thisRigidbody.velocity = Vector3.zero;

            if (touch.phase == TouchPhase.Ended)
            {
                if (actualShotCooldown <= 0)
                    Fire();
                lastRotation = transform.eulerAngles.y;
            }


        }
        else
        {
            
            thisRigidbody.angularVelocity = Vector3.zero;
            thisRigidbody.velocity = Vector3.zero;
        }
    }

    private void RotatePC()
    {
        
        if (Input.GetMouseButton(0))
        {
            //thisRigidbody.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePos = Input.mousePosition.x;
                initialMousePos = Input.mousePosition.x;
            }
            else
            {
                
                float deltaMousePos = lastMousePos - Input.mousePosition.x;
                if (Mathf.Abs(deltaMousePos) >= 20)
                    deltaMousePos = (deltaMousePos/Mathf.Abs(deltaMousePos)) * 20;
                Vector3 rotation= Vector3.one;
                if (deltaMousePos != 0)
                {
                    rotation = new Vector3(0, deltaMousePos * rotSpeed, 0);
                }
                Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);
                lastMousePos = Input.mousePosition.x;
                thisRigidbody.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
                thisRigidbody.MoveRotation(thisRigidbody.rotation * rotationSpeed);
                thisRigidbody.angularVelocity = Vector3.zero;
                thisRigidbody.velocity = Vector3.zero;
                
            }
        }
        else
        {
            thisRigidbody.rotation = Quaternion.Euler(new Vector3(0, transform.eulerAngles.y, 0));
            thisRigidbody.angularVelocity = Vector3.zero;
            thisRigidbody.velocity = Vector3.zero;
        }


    }

    private void Sight()
    {
        lineRenderer.positionCount = 2;
        sightBounceCount = 0;
        sightLenght = sightMaxLenght;
        Vector3 sightStart = sight.position;
        RaycastHit ray;
        bool raycastHit = false;
        raycastHit = Physics.Raycast(sightStart, transform.forward, out ray, sightLenght, 1 << 9, QueryTriggerInteraction.Collide);
        lineRenderer.SetPosition(0, sightStart);
        if (!raycastHit)
            lineRenderer.SetPosition(1, sightStart + transform.forward * sightLenght);
        else
            lineRenderer.SetPosition(1, ray.point);
        sightLenght -= ray.distance;
        Vector3 reflectedRaycast = Vector3.Reflect(transform.forward, ray.normal);
        while (raycastHit && sightLenght > 0 && sightBounceCount < 4)
        {
            sightBounceCount += 1;
            lineRenderer.positionCount += 1;
            Vector3 hitPos = ray.point;
            raycastHit = Physics.Raycast(ray.point, reflectedRaycast, out ray, sightLenght, 1 << 9, QueryTriggerInteraction.Collide);
            if (raycastHit)  
            {
                lineRenderer.SetPosition(sightBounceCount + 1, ray.point);
                reflectedRaycast = Vector3.Reflect(reflectedRaycast, ray.normal); 
            }
            else
                lineRenderer.SetPosition(sightBounceCount + 1, hitPos + reflectedRaycast.normalized * sightLenght);
            sightLenght -= ray.distance;

        }

        

    }

    private void MoveSideways()
    {
        

        if (Input.GetMouseButtonUp(0))
        {
            if (actualShotCooldown < 0)
                Fire();
            lastRotation = transform.eulerAngles.y;
            thisRigidbody.MovePosition(transform.position + Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                initialMousePos = Input.mousePosition.x;
                thisRigidbody.MovePosition(transform.position + Vector3.back * moveSpeed * Time.deltaTime);
            }
            else
            {
                float deltaMousePos = initialMousePos - Input.mousePosition.x;
                if (Mathf.Abs(deltaMousePos) > 50)
                    deltaMousePos = (deltaMousePos / Mathf.Abs(deltaMousePos)) * 50;
                //if (Mathf.Abs(deltaMousePos) >= 8)
                //    deltaMousePos = lastMousePos;
                //Vector3 rotation = new Vector3(0, deltaMousePos * rotSpeed, 0);
                //Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);

                Quaternion rotationAux = Quaternion.Euler(0, -deltaMousePos/ 50, 0);
                
                if (deltaMousePos == 0)
                    deltaMousePos = 0.01f;
                if (transform.eulerAngles.y < 210 && deltaMousePos < 0)
                    thisRigidbody.MoveRotation(transform.rotation * rotationAux);
                else if (transform.eulerAngles.y > 150 && deltaMousePos > 0)
                    thisRigidbody.MoveRotation(transform.rotation * rotationAux);
                thisRigidbody.MovePosition(transform.position + (Vector3.right * deltaMousePos * moveSidewaysSpeed * Time.deltaTime/27) + (Vector3.back * moveSpeed * Time.deltaTime));
                //thisRigidbody.angularVelocity = Vector3.zero;
                thisRigidbody.velocity = Vector3.zero;
            }
        }
        else
        {
            MovePlayer();
        }
    }


    public void TakeDamage()
    {
        vidas[vidas.Count - life].SetActive(false);
        life -= 1;
        if (life <= 0)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameManager.GameOver();
    }

    private void GameWin()
    {
        score += levelTime * 10;
        PlayerPrefs.SetInt("thisRunScore", (int)score);
        gameManager.GameWin();
    }

    public void EnemyKilled(GameObject enemy)
    {
        score += 80;
        aliveEnemies.Remove(enemy);
        if (aliveEnemies.Count <= 0)
            GameWin();
    }

    private void MoveSidewaysTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {

                initialMousePos = touch.position.x;
                thisRigidbody.MovePosition(transform.position + Vector3.back * moveSpeed * Time.deltaTime);
            }
            if (notSwiping)
            {
                notSwiping = false;
            }


            float deltaMousePos = initialMousePos - touch.position.x;
            if (Mathf.Abs(deltaMousePos) > 50)
                deltaMousePos = (deltaMousePos / Mathf.Abs(deltaMousePos)) * 50;
            //if (Mathf.Abs(deltaMousePos) >= 8)
            //    deltaMousePos = lastMousePos;
            //Vector3 rotation = new Vector3(0, deltaMousePos * rotSpeed, 0);
            //Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);

            Quaternion rotationAux = Quaternion.Euler(0, -deltaMousePos / 50, 0);

            if (deltaMousePos == 0)
                deltaMousePos = 0.01f;
            if (transform.eulerAngles.y < 210 && deltaMousePos < 0)
                thisRigidbody.MoveRotation(transform.rotation * rotationAux);
            else if (transform.eulerAngles.y > 150 && deltaMousePos > 0)
                thisRigidbody.MoveRotation(transform.rotation * rotationAux);
            thisRigidbody.MovePosition(transform.position + (Vector3.right * deltaMousePos * moveSidewaysSpeed * Time.deltaTime / 27) + (Vector3.back * moveSpeed * Time.deltaTime));
            //thisRigidbody.angularVelocity = Vector3.zero;
            thisRigidbody.velocity = Vector3.zero;

            if (touch.phase == TouchPhase.Ended)
            {
                if (actualShotCooldown < 0)
                    Fire();
                lastRotation = transform.eulerAngles.y;
                thisRigidbody.MovePosition(transform.position + Vector3.back * moveSpeed * Time.deltaTime);
            }


        }
        else
        {
            MovePlayer();
        }


        
    }

    private void DeathBoolOff()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("DeathBool", false);
        bloodAnim.Play();
        
        tryAgainButton.StartFadein();
    }

    private void StartDeath()
    {
        transform.GetChild(0).GetComponent<Animator>().SetBool("DeathBool", true);
        gameOverText.fadeIn = true;
        Invoke("DeathBoolOff", 2f);
    }
}
