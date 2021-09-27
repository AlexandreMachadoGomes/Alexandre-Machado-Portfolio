using UnityEngine;

public class NamePlate : MonoBehaviour
{
    public GameObject namePlate;

    void FixedUpdate()
    {
        namePlate.transform.position = gameObject.transform.position;        
    }
}