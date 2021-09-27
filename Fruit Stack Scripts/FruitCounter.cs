using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitCounter : MonoBehaviour
{
    private int fruitsOnCrate = 0;

    public IntVariable totalOranges;

    public IntVariable crateFruit;

    public CratesManager crateManager;

    public Slider counterSlider;

    public CrateBehaviour crate;

    public bool crateCountEnded = false;

    public NumberShrink numberShrink;

    public GameObject fruitCollectedParticle;

    private List<GameObject> fruits;

    private CrateBehaviour crateScript;

    public Text totalFruits;

    // Start is called before the first frame update
    void Start()
    {
        fruits = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("fruit") && !crateCountEnded)
        {
            
            Fruit fruitScript = other.gameObject.GetComponent<Fruit>();
            if (!fruitScript.orangeCount && fruits != null)
            {
                other.transform.parent = crateManager.car;
                fruits.Add(other.gameObject);
                crateFruit.Value += 1;
                totalOranges.Value += 1;
                if (crateFruit.Value >= crateManager.maxFruitCollect)
                    crateManager.ReleaseCrate();

                fruitsOnCrate += 1;
                counterSlider.value += 1;
                numberShrink.PickedFruit();
                fruitScript.caught = true;

                totalFruits.text = (int.Parse(totalFruits.text) + 1).ToString();

                Quaternion particleRot = Quaternion.Euler(-90, 0, 0);

                crateManager.cratePoints += fruitScript.points;

                if (!fruitScript.spawnedParticles)
                {
                    fruitScript.spawnedParticles = true;
                    GameObject fruitParticle = Instantiate(fruitCollectedParticle, other.transform.position + Vector3.up * .5f, particleRot);
                    Destroy(fruitParticle, 5f);
                }

                fruitScript.orangeCount = true;

                //Instantiate(textPoint, transform.position + Vector3.up * 0.3f, Quaternion.identity, canvas.transform);

                if(TutorialManager.Instance != null && int.Parse(totalFruits.text) >= 2)
                    TutorialManager.Instance.hasCollected = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("fruit") && !crateCountEnded && fruits.Contains(other.gameObject)) 
        {
            Fruit fruitScript = other.gameObject.GetComponent<Fruit>();
            if (fruitScript.orangeCount)
            {
                crateFruit.Value -= 1;
                totalOranges.Value -= 1;

                fruitsOnCrate -= 1;
                counterSlider.value -= 1;
                numberShrink.PickedFruit();

                totalFruits.text = (int.Parse(totalFruits.text) - 1).ToString();


                //Quaternion particleRot = Quaternion.Euler(-90, 0, 0);

                //GameObject fruitParticle = Instantiate(fruitCollectedParticle, other.transform.position + Vector3.up * .5f, particleRot);
                //Destroy(fruitParticle, 5f);

                fruitScript.orangeCount = false;




                //Instantiate(textPoint, transform.position + Vector3.up * 0.3f, Quaternion.identity, canvas.transform);


            }
        }
    }

    public void AddFruits()
    {
        totalOranges.Value += fruitsOnCrate;
    }

    public void RemoveFromCounter()
    {
        crateFruit.Value -= fruitsOnCrate;
        totalOranges.Value -= fruitsOnCrate;

        counterSlider.value -= fruitsOnCrate;
        numberShrink.PickedFruit();
    }

    


}


