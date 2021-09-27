using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererTweaks : MonoBehaviour
{

    private LineRenderer lineRenderer;
    private float time = 0;
    private int alphaDelta = 8;
    private bool reachedTop = false;
    private bool endLife = false;
    public Transform playerTransform;
    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            transform.localScale += Vector3.up * alphaDelta * Time.deltaTime;
            if (reachedTop)
            {
                alphaDelta *= -1;
                reachedTop = false;
                StartCoroutine(TimerBot());
            }
            else if (endLife)
                Destroy(this.gameObject);

        }
    }

    private IEnumerator TimerTop()
    {
        yield return new WaitForSeconds(2);
        reachedTop = true;
    }

    private IEnumerator TimerBot()
    {

        yield return new WaitForSeconds(2.5f);
        endLife = true;
    }

    public void StartAnim()
    {
        StartCoroutine(TimerTop());
        start = true;
    }

}
