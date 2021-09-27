using UnityEngine;

public class SmoothFollow : MonoBehaviour 
{
	public Transform target;
	public float smoothSpeed = 0.2f;
	public Vector3 offset;
    public Transform cameraEndPos;
    private Rigidbody targetRigidbody;
    private float targetInitialSpeed;
    private float initialX;
    private float initialTime;
    private bool moneyShot = false;
    public bool isJumping = false;
    private float startTime;

    private void Start()
    {
        targetRigidbody = target.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
	{
        if (moneyShot)
            TurnForMoneyShot();
        else
        {
            if (isJumping)
            {
                Vector3 desiredPos = new Vector3(-8, 33, 6f);
                Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPos;
            }
            else
            {
                Vector3 desiredPos = target.position + offset;
                Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPos;
            }
        }
	}

    private void TurnForMoneyShot()
    {

        float moneyShotMovingTimer = Time.time - startTime;
        if (moneyShotMovingTimer < 20)
        {
            Vector3 desiredPos = cameraEndPos.position;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, moneyShotMovingTimer/20);
            transform.position = smoothedPos;
        }
    }


    public void SetMoneyShot()
    {
        startTime = Time.time;
        targetInitialSpeed = targetRigidbody.velocity.magnitude;
        Debug.Log(targetInitialSpeed);
        moneyShot = true;
        initialX = transform.eulerAngles.x;
        initialTime = Time.time;
    }

}