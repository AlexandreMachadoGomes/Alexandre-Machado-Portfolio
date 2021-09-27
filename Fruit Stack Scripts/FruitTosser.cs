using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitTosser : MonoBehaviour
{

    public List<GameObject> fruit;

    public CratesManager cratemanager;

    private Vector3 dir;


    public Slider counterSlider;

    public GameObject canvas;

    public float minFruitTossTime = 0.5f;
    public float maxFruitTossTime = 1f;

    public float hitRandomOffset = .3f;


    // Start is called before the first frame update
    void Start()
    {
        dir = -transform.position;
        Invoke("TossFruit", 3f);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (cratemanager.cratesList.Count > 0)
            MoveHeightToCrate();
        
    }


    private void TossFruit()
    {
        if (cratemanager.cratesList.Count > 0 
            && cratemanager.cratesList[cratemanager.cratesList.Count - 1].GetComponent<CrateBehaviour>().rotate)
        {
            if(TutorialManager.Instance != null && TutorialManager.Instance.currentStep != TutorialManager.tutorialStep.COLLECT) return;

            float randomX = Random.Range(-Mathf.Sqrt(20f), Mathf.Sqrt(20f));
            if (randomX == 0)
                  randomX = 0.001f;
            float randomZ = Mathf.Sqrt((20 / Mathf.Pow(randomX, 2)));

            Vector3 newPos;
            if (Random.Range(0, 1) > 0)
                newPos = new Vector3(randomX, transform.position.y, randomZ);
            else
                newPos = new Vector3(randomX, transform.position.y, -randomZ);

            int randomFruit = Random.Range(0, fruit.Count);

            GameObject newFruit = Instantiate(fruit[randomFruit], newPos, Quaternion.identity);


            float randomTorque = Random.Range(0.3f, 2f);
            newFruit.GetComponent<Rigidbody>().AddTorque(Vector3.forward * randomTorque/10, ForceMode.Impulse);

            Fruit fruitScript = newFruit.GetComponent<Fruit>();
            fruitScript.target = cratemanager.cratesList[cratemanager.cratesList.Count - 1].transform;
            fruitScript.crateManager = cratemanager;
            fruitScript.canvas = this.canvas;
            fruitScript.hitRandomOffset = hitRandomOffset;

        }

        float randomFruitTime = Random.Range(minFruitTossTime, maxFruitTossTime);
        Invoke("TossFruit", randomFruitTime);
    }

    private void MoveHeightToCrate()
    {
        transform.position = new Vector3 (transform.position.x, cratemanager.cratesList[cratemanager.cratesList.Count - 1].transform.position.y + .5f, transform.position.z);
    }

}
