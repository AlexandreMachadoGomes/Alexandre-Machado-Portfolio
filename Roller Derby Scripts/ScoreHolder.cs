using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHolder : MonoBehaviour
{

    public List<GameObject> score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartScore()
    {
        StartCoroutine(StartScore2());
    }

    private IEnumerator StartScore2()
    {
        yield return new WaitForSeconds(4f);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ScoreDisplayer>().DisplayScore();
        }
    }
}
