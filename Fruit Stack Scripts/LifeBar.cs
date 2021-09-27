using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBar : MonoBehaviour
{

    public List<GameObject> vidas;

    public IntVariable playerLife;

    public GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        playerLife.Value = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void LooseLife()
    {
        playerLife.Value -= 1;

        if (playerLife.Value <= 0)
        {
            GameOver();
        }

        GameObject lostLife = vidas[vidas.Count - 1];
        vidas.Remove(lostLife);
        Destroy(lostLife);

    }

    private void GameOver()
    {
        gameManager.GameOver();
    }
}
