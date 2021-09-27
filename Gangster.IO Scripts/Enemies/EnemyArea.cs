using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArea : MonoBehaviour
{
    public GameObject enemy;
    private Enemy enemyScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyScript = enemy.GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!enemyScript.isSeeingPlayer)
            enemyScript.isSeeingPlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (enemyScript.isSeeingPlayer)
            enemyScript.isSeeingPlayer = false;
    }
}
