using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveAndRotateO : MonoBehaviour
{
    public bool arrived = false;    
    public float speed = 3;
    public Transform arrivePos;

    public float fadeInTime = 3;
    private float timer = 0;

    public Text gameTitle;
    public Text gameTitleShadow;

    public FadeIn gameStartText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(transform.position.x);
        if (!arrived)
            MoveAndRotate();
        else if (timer < fadeInTime)
            FadeInTitle();
    }

    private void MoveAndRotate()
    {
        if ((transform.position.x) >= arrivePos.position.x)
        {
            arrived = true;
        }

        transform.Rotate(Vector3.back * speed);
        transform.position = transform.position + Vector3.right * ((2*Mathf.PI*41.5f)/360) * speed;
    }

    private void FadeInTitle()
    {
        timer += Time.deltaTime;
        Color newColor = gameTitle.color;
        newColor.a = timer / fadeInTime;
        gameTitle.color = newColor;

        if (timer >= fadeInTime)
        {
            gameTitleShadow.gameObject.SetActive(true);
            gameStartText.StartFadeIn();
            gameObject.SetActive(false);
        }

    }

}
