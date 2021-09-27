using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEffectArea : MonoBehaviour
{

    public List<LineRendererTweaks> lines;
    public List<PlaneMoneyShot> planes;
    private bool activated = false;
    public int scoreValue = 1;

    public GameObject confettiObj;
    public Animator spriteScoreFlashAnim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            other.GetComponent<ScoreDisplayer>().thisScore = scoreValue;
        /*if (!activated)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i].StartAnim();

            }
            for (int i = 0; i < planes.Count; i++)
            {
                planes[i].StartAnim();
            }
            activated = true;
        }*/
    }

    public void BlockReachedHere()
    {
        confettiObj.SetActive(true);
        spriteScoreFlashAnim.Play("ssf");
    }
}
