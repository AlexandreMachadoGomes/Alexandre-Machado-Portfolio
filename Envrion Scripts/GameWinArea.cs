using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinArea : MonoBehaviour
{

    public GameManager gameManager; 

    private void OnTriggerEnter(Collider other)
    {
        gameManager.GameWin();
    }

}
