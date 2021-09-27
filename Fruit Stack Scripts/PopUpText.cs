using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    public float speed = 1;

    public float popUpTime = 1;

    public Text popUpText;

    private bool donePopUp = false;

    // Start is called before the first frame update
    void Start()
    {
        popUpText = GetComponent<Text>();
        Destroy(gameObject, popUpTime + 0.4f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUp();
        if (!donePopUp)
            PopUp();
    }

    private void PopUp()
    {
        if (transform.localScale.magnitude <= 1)
        {
            transform.localScale = Vector3.one * Mathf.Lerp(1, 2, Mathf.Pow(popUpTime, 2));
            popUpTime -= Time.deltaTime;
        }
        else
        {
            donePopUp = true;
            popUpText.CrossFadeAlpha(0, 1, false);
        }

    }

    private void MoveUp()
    {
        transform.position += Vector3.up * speed;
    }

}
