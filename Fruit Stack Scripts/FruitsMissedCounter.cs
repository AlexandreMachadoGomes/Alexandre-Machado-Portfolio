using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitsMissedCounter : MonoBehaviour
{

    public CratesManager cratesManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fruit"))
        {
            Fruit fruitScript = other.GetComponent<Fruit>();
            if (!fruitScript.caught)
            {
                fruitScript.caught = true;
                cratesManager.DealNegativePoints();
            }
        }
    }

}
