using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public int hitPoints;
    public float fireCooldownMax;
    private float cooldown;
    public GameObject Player;
    public Player playerScript;
    public bool isSeeingPlayer;
    public GameObject projectile;
    private Rigidbody thisRigidbody;
    public float rotationVelocity;
    private Vector3 rotationDirection = Vector3.down;
    public GameObject restDir;
    private Animator animator;
    public bool dead = false;
    public GameObject slider;
    private Slider sliderComponent;
    public float timeMultiplier;
    public GameObject deathBubbles;
    public Animator camAnim, flashAnim;
    public GameManager2 gm;
    private float originalFixedTimeScale;
    private float originalTimeScale;

    public Transform sight;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        //sliderComponent = slider.GetComponent<Slider>();
        //sliderComponent.maxValue = hitPoints;
        //sliderComponent.value = hitPoints;
        playerScript = Player.GetComponent<Player>();

        originalFixedTimeScale = Time.fixedDeltaTime;
        originalTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {


        if (dead)
        {
            Time.timeScale += (1f / 2f) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0, originalTimeScale);

            Time.fixedDeltaTime += (1f / 2f) * Time.unscaledDeltaTime;
            Time.fixedDeltaTime = Mathf.Clamp(Time.fixedDeltaTime, 0, originalFixedTimeScale);
        }
        else
        {

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
            {


            }
            cooldown -= Time.deltaTime;

            if (isSeeingPlayer)
            {
                LookAt(playerScript.body.gameObject);
                Vector3 projectileSpawnPos = new Vector3(0, 1.5f, 0);
                bool lineOfSight = Physics.Raycast(transform.position + transform.forward + projectileSpawnPos, transform.position + transform.forward * 2 + projectileSpawnPos, LayerMask.GetMask("Player"));
                if (cooldown <= 0)
                {
                    Fire();
                }
            }
            else
                LookAt(restDir);
        }
    }

    private void DestroyThis()
    {
        playerScript.EnemyKilled(gameObject);
        gm.enemies.Remove(this);
        Destroy(this.gameObject);
    }


    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Projectile" && !dead)
        {
            
            Destroy(collision.gameObject);
            

            hitPoints -= 1;
            //sliderComponent.value -= 1;
            if (hitPoints <= 0)
            {
                
                dead = true;

                Invoke("DestroyThis", 3);
                //Destroy(slider.gameObject);
                animator.SetBool("Dead", true);
                Time.timeScale = timeMultiplier;
                originalFixedTimeScale = Time.fixedDeltaTime;
                Time.fixedDeltaTime = Time.timeScale * .02f;
                //deathBubbles.SetActive(true);
                camAnim.Play("csm", -1, 0.0f);
                flashAnim.Play("flash", -1, 0.0f);
                //gm.PopKillTextEffect();
            }
            
        }
    }


    private void Fire()
    {
        Vector3 projectileSpawnPos = new Vector3(0, 1.5f, 0);
        Instantiate(projectile, sight.position, transform.rotation);
        cooldown = fireCooldownMax;
    }

    private void LookAt(GameObject thing)
    {
        float aux = Vector3.Angle(transform.forward, thing.transform.position - transform.position);

        if (aux > 4)
        {
            animator.SetFloat("Blend", 0.8f);
            //Vector3 rotation = new Vector3(0, deltaMousePos * 6, 0);
            //Quaternion rotationSpeed = Quaternion.Euler(rotation * Time.deltaTime);
            float desiredAngle = Quaternion.LookRotation(thing.transform.position - transform.position, transform.up).eulerAngles.y;

            float angle = transform.eulerAngles.y - desiredAngle;
            //if (angle > 0)
            if (Mathf.Abs(angle) < 180)
            {
                if (desiredAngle > transform.eulerAngles.y)
                    rotationDirection = Vector3.up;
                else
                    rotationDirection = Vector3.down;
            }
            else
            {
                if (desiredAngle > transform.eulerAngles.y)
                    rotationDirection = Vector3.down;
                else
                    rotationDirection = Vector3.up;
            }
            Quaternion deltaRotation = Quaternion.Euler(rotationVelocity * Time.deltaTime * rotationDirection);


            thisRigidbody.MoveRotation(thisRigidbody.rotation * deltaRotation);

            
        }
        else
            animator.SetFloat("Blend", 0);
    }



}