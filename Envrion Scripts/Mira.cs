using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mira : MonoBehaviour
{

    public float initSize;
    public float finalSize;
    private float startTime;
    private Vector2 vInit;
    private Vector2 vFinal;
    public float timeLerpAim;
    public bool goTime;
    private bool lastDigit;
    private GameObject quadAim;



    // Start is called before the first frame update
    void Start()
    {
        vInit = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (goTime)
        {
            float lerp_factor = (Time.time - startTime) / timeLerpAim;

            if (lerp_factor >= 1)
            {
                this.transform.gameObject.SetActive(false);
            }

            if (!lastDigit) 
            {
                float lerpAngle = Mathf.LerpAngle(0, 90, lerp_factor);

                transform.localEulerAngles = new Vector3(0, 0, lerpAngle);

                /// Lerp da escala do quadrado
                float lerpScale = Mathf.Lerp(initSize, finalSize, lerp_factor);

                transform.localScale = new Vector3(lerpScale, lerpScale, 1);

                /// Lerp da posição
                float lerp_pos_x = Mathf.Lerp(vInit.x, vFinal.x, lerp_factor);
                float lerp_pos_y = Mathf.Lerp(vInit.y, vFinal.y, lerp_factor);

                Vector3 finalPosition = Vector3.zero;
                finalPosition.x = lerp_pos_x;
                finalPosition.y = lerp_pos_y;

                transform.localPosition = finalPosition;
            }

        }
    }

    public void SetMira(Vector2 vFinal2, float timeLerpAim2, bool lastDigit2, GameObject quadAim2)
    {
        vFinal = vFinal2;
        timeLerpAim = timeLerpAim2;
        goTime = true;
        startTime = Time.time;
        lastDigit = lastDigit2;
        quadAim = quadAim2;
    }


}
