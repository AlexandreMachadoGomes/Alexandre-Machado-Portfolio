using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardFollow : MonoBehaviour
{
    //public float moveSpeed;
    public Transform target;
    public Vector3 offset = Vector3.zero;
    private Rigidbody thisRigibody;
    public bool followWithPhysics = false;
    public bool rotate = false;
    public bool bot = false;
    public bool isPlayer;

    // Start is called before the first frame update
    void Start()
    {
        thisRigibody = GetComponent<Rigidbody>(); 
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = target.position + offset;
        if ((transform.position - pos).magnitude > 0.1f && followWithPhysics)
        {
            if (!isPlayer)
            {
                thisRigibody.MovePosition(pos * 10);
            }
            else
            {
                thisRigibody.MovePosition(pos * 15);
            }
        }
        else
            transform.position = pos;

    }


    public void SetRotation(float rotation)
    {
        
    }
}
