using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    private Rigidbody thisRigidBody;
    public float shotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        thisRigidBody = this.GetComponent<Rigidbody>();
        thisRigidBody.AddForce(Vector3.up * 0.0001f, ForceMode.Impulse);
        Invoke("GetDestroyed", 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveShot();
    }

    private void MoveShot()
    {
        thisRigidBody.MovePosition(transform.position + Vector3.forward * shotSpeed * Time.deltaTime);
    }

    public void GetDestroyed()
    {
        Destroy(this.gameObject);
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(this.gameObject);
    }
}
