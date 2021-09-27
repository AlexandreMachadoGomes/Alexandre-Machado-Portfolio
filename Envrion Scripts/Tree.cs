using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{

    public float treeMoveSpeed;
    public float treeRotationSpeed;
    private Rigidbody thisRigidBody;
    private bool gameIsOver;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidBody = this.GetComponent<Rigidbody>();
        gameIsOver = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!gameIsOver) 
        {
            MoveTree();
        }
    }

    //Moves and rotates the tree
    private void MoveTree()
    {
        thisRigidBody.MovePosition(transform.position + Vector3.back * treeMoveSpeed * Time.deltaTime);
        Quaternion deltaRotation = Quaternion.Euler(Vector3.up * treeRotationSpeed * Time.deltaTime);
        this.thisRigidBody.MoveRotation(thisRigidBody.rotation * deltaRotation);
    }




    public void GameOver()
    {
        {
            gameIsOver = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            GameObject.Find("Vidas").GetComponent<LifeManager>().LoosePlayerLife();
        }
    }
}
