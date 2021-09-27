using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hostage : MonoBehaviour
{
    private Rigidbody thisRigidBody;
    private Collider thisCollider;
    private float lerpTimeStartAux;
    public float runFromTreeAngle;
    public GameObject runFromTreePosObject;
    private Vector3 runFromTreePos;
    public bool isFree = false;
    public bool isWithPlayer = false;
    public bool goingToPlayer = false;
    public bool waitingForPlayer = false;
    private bool playerPassed = false;
    public GameObject player;
    private Player playerScript;
    public int hostageNumber;
    private Animator thisAnim;
    public bool runDirectionIsRight;
    private bool runningToSide;
    private Vector3 posRunSide;
    private float treePos;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidBody = this.GetComponent<Rigidbody>();
        thisRigidBody.detectCollisions = false;
        thisCollider = this.GetComponent<Collider>();
        thisCollider.enabled = false;
        playerScript = player.GetComponent<Player>();
        thisAnim = GetComponent<Animator>();
        runFromTreePos = runFromTreePosObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isWithPlayer)
        {
            if (isFree)
            {
                if (!goingToPlayer && !waitingForPlayer)
                {
                    GoToTheSide();
                }
                else if (!goingToPlayer && waitingForPlayer)
                {
                    WaitForPlayer();
                }
                else if (goingToPlayer && !waitingForPlayer)
                {
                    GoToPlayer();
                }
            }
        }
        else
        {
            thisRigidBody.MovePosition(transform.position + Vector3.forward * playerScript.playerMoveSpeed * Time.deltaTime);
            if (Mathf.Abs(transform.position.x - treePos) > 0.1f)
            {
                float posX = 1;
                if (transform.position.x - treePos > 0)
                    posX = -1;
                Vector3 pos = new Vector3(posX, 0, 1) * playerScript.playerMoveSpeed * Time.deltaTime;
                thisRigidBody.MovePosition(transform.position + pos);
            }
        }
    }


    public void BrokeFree()
    {
        thisAnim.SetBool("isFree", true);
        isFree = true;
        thisRigidBody.isKinematic = false;
        thisRigidBody.useGravity = true;
        thisRigidBody.detectCollisions = true;
        thisRigidBody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        thisCollider.enabled = true;
        thisCollider.isTrigger = false;
        lerpTimeStartAux = Time.time;
    }

    private void GoToTheSide()
    {
        float angle = transform.rotation.eulerAngles.y;
        if (angle < runFromTreeAngle - 1 || angle > runFromTreeAngle + 1)
        {
            float lerpFactor = (Time.time - lerpTimeStartAux) / 0.5f;
            float presentAngle = Mathf.LerpAngle(angle, runFromTreeAngle, lerpFactor);
            transform.eulerAngles = new Vector3(0, presentAngle, 0);
        }
        if (Vector3.Distance(transform.position, runFromTreePos) > 0.5f)
        {
            float lerpFactor = (Time.time - lerpTimeStartAux) / 1f;
            thisAnim.SetFloat("BlendEscapeTree", lerpFactor);
            float presentPosx = Mathf.Lerp(transform.position.x, runFromTreePos.x, lerpFactor);
            float presentPosz = Mathf.Lerp(transform.position.z, runFromTreePos.z, lerpFactor);
            transform.position = new Vector3(presentPosx, transform.position.y, presentPosz);

        }
        else
        {
            thisAnim.SetBool("isWaiting", true);
            waitingForPlayer = true;
            lerpTimeStartAux = Time.time;
        }
    }

    private void WaitForPlayer()
    {
        float angle = transform.rotation.eulerAngles.y;
        float angleAux = -270;
        if (runDirectionIsRight)
            angleAux *= -1;
        if (angle > angleAux+ 1 || angle < angleAux - 1)
        {
            float lerpFactor = (Time.time - lerpTimeStartAux) / 0.5f;
            float presentAngle = Mathf.LerpAngle(angle, angleAux, lerpFactor);
            transform.eulerAngles = new Vector3(0, presentAngle, 0);
        }
        
        if (transform.position.z <= player.transform.position.z)
        {
            goingToPlayer = true;
            waitingForPlayer = false;
            playerScript.hostages.Add(this.gameObject);
            hostageNumber = playerScript.hostages.Count;
        }
    }


    private void GoToPlayer()
    {
        float intendedPos = player.transform.position.z  -.3f * hostageNumber;
        float angle = transform.rotation.eulerAngles.y;

        

        if (transform.position.z <= intendedPos)
        {

            if (!playerPassed)
            {
                lerpTimeStartAux = Time.time;
                thisAnim.SetBool("goingToPlayer", true);
                playerPassed = true;
            }

            thisRigidBody.MovePosition(transform.position + Vector3.forward * playerScript.playerMoveSpeed * Time.deltaTime);

            if (angle > 1 || angle < 359)
            {
                float lerpFactor = (Time.time - lerpTimeStartAux) / 0.75f;
                float presentAngle = Mathf.LerpAngle(angle, 360, lerpFactor);
                transform.eulerAngles = new Vector3(0, presentAngle, 0);
            }

            if (Mathf.Abs(transform.position.x -player.transform.position.x) > 0.1f)
            {
                float lerpFactor = (Time.time - lerpTimeStartAux) / 20;
                float presentPos = Mathf.Lerp(transform.position.x, player.transform.position.x, lerpFactor);
                thisAnim.SetFloat("BlendMovePlayer", lerpFactor);
                transform.position = new Vector3(presentPos, transform.position.y, transform.position.z);
            }
            else
            {
                treePos = player.transform.position.x;
                thisAnim.SetBool("isWithPlayer", true);
                isWithPlayer = true;
            }
        }        
    }




    public void JumpWithPlayer()
    {

        thisAnim.SetTrigger("Jump");
        
    }

    public IEnumerator startRunSide(float nextTreePos)
    {
        yield return new WaitForSeconds(0.5f);
        runningToSide = true;
        treePos = nextTreePos;
    }

    
}
