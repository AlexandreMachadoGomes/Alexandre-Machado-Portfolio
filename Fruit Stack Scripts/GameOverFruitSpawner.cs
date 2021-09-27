using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverFruitSpawner : MonoBehaviour
{

    public List<GameObject> fruits;

    public float range = 2;

    public bool stopSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        SpawnFruit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFruit()
    {
        Vector3 randomPos = transform.right * Random.Range(-range, range);
        Vector3 spawnPos = transform.position + randomPos;
        //spawnPos.x = transform.position.x + Random.Range(-radius, radius);
        Instantiate(fruits[Random.Range(0, fruits.Count)], spawnPos, Quaternion.identity);
        if (!stopSpawning)
            Invoke("SpawnFruit", .1f);
    }
}
