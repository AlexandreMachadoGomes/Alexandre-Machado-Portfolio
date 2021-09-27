using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootArea : MonoBehaviour
{
    // Start is called before the first frame update

    public Player player;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        player.enemyTargetList.Add(other.transform);
        other.GetComponent<Bot>().guyAnim.transform.GetChild(1).GetComponent<Outline>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        player.enemyTargetList.Remove(other.transform);
        other.GetComponent<Bot>().guyAnim.transform.GetChild(1).GetComponent<Outline>().enabled = false;
    }
}
