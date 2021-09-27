using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trunk : MonoBehaviour
{

    public TreeTrunk treeTrunk;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            Destroy(other.gameObject);
            treeTrunk.LooseLife();
        }
        else
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().GameOver();
        }
    }
}
