using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeManager : MonoBehaviour
{

    public GameManager gameManager;
    private int lifeTotal;

    public List<GameObject> vidas;

    // Start is called before the first frame update
    void Start()
    {
        lifeTotal = vidas.Count;
        for (int i = 0; i < lifeTotal; i++)
        {
            vidas[i].SetActive(true);
        }
        
    }

    public void LoosePlayerLife()
    {
        vidas[lifeTotal -1].SetActive(false);
        lifeTotal -= 1;
        if (lifeTotal <= 0)
        {
            gameManager.GameOver();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
