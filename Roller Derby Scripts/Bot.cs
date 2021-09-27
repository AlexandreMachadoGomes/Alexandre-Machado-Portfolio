using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public float botSpeed = 1;
    public Transform[] dir;
    public float rotateSpeed = 1;
    public int dirCount = 0;
    private Rigidbody thisRigidbody;
    public MagneticField playerMagneticField;
    public Color botColor;
    public Player player;
    private bool isJumping = false;
    public Animator guyAnim;
    public bool startAnim = false;
    public float startAnimTime = 0;
    private bool aux = false;
    public Transform plane;
    private bool gotHit = false;
    public List<BallProjectile> followingBalls;
    private Vector3 offset;
    private float thisVel = 0;
    private float rotationValue = 0;
    private Vector3 notMovingOffset;
    private float deltaVel;
    private float previousVel;


    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(0, -0.35f, -1.2f);
        notMovingOffset = offset;
        thisRigidbody = GetComponent<Rigidbody>();
        deltaVel = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        previousVel = thisVel;
        thisVel = thisRigidbody.velocity.x;

        RotateGuy();
        if (!gotHit)
        {

            
            if (thisVel == 0)
                thisVel = 0.01f;


            if (dir[dirCount].position.z < transform.position.z && dirCount < dir.Length - 1)
                dirCount += 1;
            thisRigidbody.AddForce(Vector3.down * 800 * botSpeed);
            Vector3 direction = Vector3.Project(dir[dirCount].position - transform.position, Vector3.right);
            if (Mathf.Abs(transform.position.x - dir[dirCount].position.x) > 0.5f)
                thisRigidbody.AddForce(direction.normalized * 50, ForceMode.Force);

            if (thisVel == 0)
                thisVel = 0.01f;



        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("damage"))
        {
            Explode(collision.GetContact(0).normal);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            
            Transform target = other.transform.parent.GetChild(0);
            Rigidbody targetRig = target.GetComponent<Rigidbody>();
            target.GetComponent<Collider>().isTrigger = true;
            targetRig.isKinematic = true;
            target.gameObject.layer = 0;
            //playerMagneticField.RemoveFromList(target);
            targetRig.detectCollisions = false;
            other.transform.parent.SetParent(transform);
        }
    }

    public void Jump()
    {
        this.thisRigidbody.drag = 0;
        thisRigidbody.velocity = new Vector3(0, thisRigidbody.velocity.y, thisRigidbody.velocity.z);
        thisRigidbody.AddForce((thisRigidbody.velocity.normalized + player.jumpForce) * 0.3f, ForceMode.Impulse);
        guyAnim.gameObject.SetActive(false);
        isJumping = true;
        RenderSettings.fogDensity = 0.005f;
        StartCoroutine(jumped());
    }


    public void Explode(Vector3 dir)
    {
        while (0 < transform.childCount)
        {
            Transform target = transform.GetChild(0).GetChild(0);
            Vector3 explosionDir = transform.GetChild(0).transform.position - transform.position;
            if (explosionDir.z < 0.10f)
                explosionDir.z = 0.10f;
            Rigidbody targetRig = target.GetComponent<Rigidbody>();
            Collider targetCol = target.GetComponent<Collider>();
            target.gameObject.layer = 18;
            targetRig.isKinematic = false;
            targetRig.AddForce(explosionDir.normalized + dir * -0.5f, ForceMode.Impulse);
            targetRig.detectCollisions = true;
            targetCol.material = player.bouncyMaterial;
            targetCol.isTrigger = false;
            transform.GetChild(0).parent = null;
        }
        this.GetComponent<MeshRenderer>().enabled = false;
        thisRigidbody.isKinematic = true;
        guyAnim.gameObject.SetActive(false);
        player.enemyTargetList.Remove(transform);

        for (int i = 0; i < followingBalls.Count; i++)
        {
            followingBalls[i].target = null;
        }
        Debug.Log("enemy  " + this.name + "  died");
        Destroy(this.gameObject);
    }


    private IEnumerator jumped()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<SphereCollider>().radius = 1;
    }

    public IEnumerator KickAnimStartTimer()
    {
        yield return new WaitForSeconds(3);
        startAnim = true;
        guyAnim.SetBool("startKick", true);
    }

    public void GetHit(Vector3 dir)
    {
        thisRigidbody.drag = 0.5f;
        Vector3 dirAux = Vector3.ProjectOnPlane(dir, plane.up);
        dirAux.x *= 1;
        gotHit = true;
        thisRigidbody.AddForce(dirAux * 10, ForceMode.Impulse);
            
    }

    private void RotateGuy()
    {
        Transform guyTransform = guyAnim.transform;

        if (Mathf.Abs(thisVel) > player.maxVelocity* 3)
            thisRigidbody.velocity = new Vector3((thisVel / Mathf.Abs(thisVel)) * 3 * player.maxVelocity, thisRigidbody.velocity.y, thisRigidbody.velocity.z);


        deltaVel = thisVel - previousVel;


        guyTransform.position = transform.position + notMovingOffset;
        guyTransform.RotateAround(transform.position, Vector3.up, deltaVel * 8 * rotateSpeed);
        notMovingOffset = guyTransform.position - transform.position;

    }

}
