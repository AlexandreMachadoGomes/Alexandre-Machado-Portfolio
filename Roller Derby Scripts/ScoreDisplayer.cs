using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayer : MonoBehaviour
{

    private List<GameObject> score;
    public int thisScore = 0;
    private Transform player;
    private Rigidbody thisRigidbody;
    private bool deactivateScore = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        thisRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.z > transform.position.z + 10 && !thisRigidbody.isKinematic && !player == null)
        {
            thisRigidbody.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 14)
        {
            deactivateScore = true;
        }   
    }

    public void DisplayScore()
    {
        if (!deactivateScore)
        {
            score = transform.parent.GetComponentInParent<ScoreHolder>().score;
            if (thisScore > 10)
                thisScore = 10;
            GameObject scoreNumb = Instantiate(score[thisScore], transform.position + Vector3.up * 2, Quaternion.identity);
            player.GetComponent<Player>().totalScore += thisScore * 10;
            //Instantiate(score[0], scoreNumb.transform.position + Vector3.right * 2, Quaternion.identity);
        }
    }

    public IEnumerator setParent(Transform parent)
    {
        yield return new WaitForSeconds(5);
        if (transform.parent != player )
            if (!transform.parent.CompareTag("Bot"))
                transform.SetParent(parent);     
    }
}
