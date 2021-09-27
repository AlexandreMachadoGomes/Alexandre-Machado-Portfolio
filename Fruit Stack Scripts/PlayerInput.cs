using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour
{

    private Vector3 mouseStartPos;

    public CratesManager crateManager;

    public bool gameOver = false;

    void FixedUpdate()
    {
        if((Input.GetMouseButton(0) || Input.touchCount > 0) && !gameOver)
        {
            if(mouseStartPos == Vector3.zero) // If pressing just started
            {
                if (crateManager.cratesList.Count > 0)
                {
                    if (crateManager.cratesList[crateManager.cratesList.Count -1].GetComponent<CrateBehaviour>().onGround)
                    {
                        mouseStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                        //Debug.Log("started input");

                        CratesManager.Instance.SpawnCrate();
                    }
                }
                else  
                {
                    mouseStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10));
                    CratesManager.Instance.SpawnCrate();
                }

            }
            else // If pressing is happening
            {
                // Call for rotation speed changes
            }
        }
        else
        {
            if(mouseStartPos != Vector3.zero ) // If pressing just ended
            {
                //Debug.Log("released input");

                CratesManager.Instance.ReleaseCrate();
                mouseStartPos = Vector3.zero;
            }
        }
        
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
