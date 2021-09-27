using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothSpeed = 0.2f;
	public Vector3 offset;
	public float offsetZ;

	public float timerTilCameraTurn = 1;


    private void Start()
    {
		offsetZ = offset.z;

	}

    void FixedUpdate()
	{
		Vector3 desiredPos = target.position + offset;


		Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
		transform.position = smoothedPos;
		RotateCamera();
	}

	private void RotateCamera()
    {
		float yAngle = Mathf.Abs(target.rotation.eulerAngles.y - 180);
		if (30 < yAngle && yAngle < 150) { 
			float cameraYRotation = (yAngle-30)/6 + 69;
			transform.rotation = Quaternion.Euler(cameraYRotation, 180, 0);
			offset.z = offsetZ + (yAngle - 30) / 30;
		}
    }

}