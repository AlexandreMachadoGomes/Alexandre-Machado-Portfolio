using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody thisRigidBody;
    public float playerMoveSpeed;
    public float jumpForce;
    public Vector3 shotSpawnOffset = Vector3.zero;
    public GameObject shot;
    public List<GameObject> hostages;
    private Animator thisAnim;
    public GameObject particleEmmiter;
    private float timeLastShot = 0;
    public List<GameObject> trees;
    public int treesDestroyed = 0;
    private float nextTreePos;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidBody = this.GetComponent<Rigidbody>();
        thisAnim = this.GetComponent<Animator>();
        nextTreePos = trees[0].transform.position.x;
        trees[0].GetComponent<TreeTrunk>().PrepareTarget();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovePlayer();


        // Jump
        //if (Input.GetButtonDown("Fire2")){
        //    Jump();
        //}
    }

    private void MovePlayer()
    {
        if (Mathf.Abs(transform.position.x - nextTreePos) > 0.1f)
        {
            float posX = 1;
            if (transform.position.x - nextTreePos > 0)
                posX = -1;
            Vector3 pos = new Vector3(posX, 0, 1) * playerMoveSpeed * Time.deltaTime * 2;
            thisRigidBody.MovePosition(transform.position + pos);
        }
        else
        {
            if (Input.GetButtonDown("Fire1") || Input.touchCount > 0)
                Shoot();
            thisRigidBody.MovePosition(transform.position + Vector3.forward * playerMoveSpeed * Time.deltaTime);
        }

    }

    public void Jump()
    {
        if (thisRigidBody.velocity.y <= 0.01f)
        {
            thisAnim.SetTrigger("Jump");
            thisRigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            for (int i = 0; i < hostages.Count; i++)
            {
                hostages[i].GetComponent<Hostage>().JumpWithPlayer();
            }
        }
    }

    public void Shoot()
    {
        if (Time.time - timeLastShot > 1)
        {
            particleEmmiter.GetComponent<ParticleSystem>().Play();
            Instantiate(shot, transform.position + shotSpawnOffset, Quaternion.identity);
            timeLastShot = Time.time;
        }
    }

    public void TreeDestroyed()
    {
        treesDestroyed += 1;
       
        if (treesDestroyed < trees.Count)
        {
           nextTreePos = trees[treesDestroyed].transform.position.x;
           trees[treesDestroyed].GetComponent<TreeTrunk>().PrepareTarget();
        }
        for (int i = 0; i < hostages.Count; i++)
        {
            StartCoroutine(hostages[i].GetComponent<Hostage>().startRunSide(nextTreePos));
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Ground"))
        {
            gameManager.GameOver();
        }
    }

}
