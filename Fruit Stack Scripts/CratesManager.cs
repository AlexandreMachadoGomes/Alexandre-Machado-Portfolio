using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CratesManager : MonoBehaviour
{
    public Transform car;

    public static CratesManager Instance;

    public GameObject cratePrefab;
    public List<GameObject> cratesList;

    public float crateFallSpeed = 1;

    [Space(5)]
    public Vector3 initialPos;

    public float releaseCrateTimeoutTime = 5;

    [Space(10)]
    public IntVariable cratesStackCount;

    private Coroutine releaseCrateCoroutine;

    public GameObject lastCrateOnPile;

    public int maxFruitCollect = 20;

    public IntVariable thisCrateFruit;

    public int fruitAmmountForWin = 100;

    public Slider counterSlider;

    public SliderCounter ActualCounterSlider;

    public LifeBar lifeBar;

    public GameManager gameManager;

    public NumberShrink numberShrink;

    public Transform canvas;

    public bool canSpawnCrate = true;

    public Text totalfruits;

    //points
    public Transform pointsTextPos;
    public float cratePoints;
    public GameObject textPoints;
    public GameObject points;
    public FloatVariable pointsVariable;

    public float crateMissAngle = 15;
    public float crateGoodAngle = 10;
    public float cratePerfectAngle = 5;

    void Awake()
    {
        pointsVariable.Value = 0;

        if (CratesManager.Instance == null)
            CratesManager.Instance = this;

        cratesStackCount.Value = 0;
        thisCrateFruit.Value = 0;
    }

    public void SpawnCrate()
    {
        if (canSpawnCrate)
        {

            if (cratesList == null)
                cratesList = new List<GameObject>();

            Vector3 spawnPos = initialPos;

            if (cratesList.Count > 0)
            {
                spawnPos += cratesList[cratesList.Count - 1].transform.position;
                spawnPos.y += cratesList[cratesList.Count - 1].GetComponent<CrateBehaviour>().cratesMergedCount * .4f;
            }

            float spawnAngle = Random.Range(-90, 90);
            Quaternion spawnRot = Quaternion.Euler(-90, 0, spawnAngle);

            GameObject crateClone = Instantiate(cratePrefab, spawnPos, spawnRot, transform);
            crateClone.transform.parent = car;

            releaseCrateCoroutine = StartCoroutine(ReleaseCrateTimer());

            FruitCounter fruitCounter = crateClone.transform.GetChild(1).GetChild(0).GetComponent<FruitCounter>();
            fruitCounter.crateManager = this;
            fruitCounter.counterSlider = counterSlider;
            fruitCounter.numberShrink = numberShrink;

            CrateBehaviour crateScript = crateClone.GetComponent<CrateBehaviour>();
            crateScript.crateDownSpeed = crateFallSpeed;
            crateScript.cantMerge = false;
            crateScript.lifeBar = lifeBar;
            crateScript.goodMergeAngle = crateGoodAngle;
            crateScript.perfectMergeAngle = cratePerfectAngle;
            crateScript.missAngle = crateMissAngle;
            crateScript.canvas = canvas;
            crateScript.ActualCounterSlider = ActualCounterSlider;
            crateScript.totalPoints = totalfruits;
            crateScript.car = car;

            fruitCounter.crate = crateScript;
            fruitCounter.totalFruits = totalfruits;

            Camera.main.GetComponent<CameraMovement>().targetTransform = crateClone.transform;

            cratesList.Add(crateClone);

            if (cratesList.Count == 1)
            {
                lastCrateOnPile = crateClone;
                crateScript.cantMerge = true;
            }

            cratesStackCount.Value += 1;
        }
    }

    public void ReleaseCrate()
    {
        if(TutorialManager.Instance != null && TutorialManager.Instance.currentStep != TutorialManager.tutorialStep.RELEASE) return;

        if(TutorialManager.Instance != null)
            TutorialManager.Instance.hasStacked = true;

        if (cratesList.Count > 0)
        {
            lastCrateOnPile = cratesList[cratesList.Count - 1];
            Rigidbody crateBody = lastCrateOnPile.GetComponent<Rigidbody>();
            CrateBehaviour crate = lastCrateOnPile.GetComponent<CrateBehaviour>();
            
            if (crate.rotate)
            {
                crate.crateManager = this;

                //disables picking up fruit after crate falls
                lastCrateOnPile.transform.GetChild(1).GetChild(0).GetComponent<FruitCounter>().crateCountEnded = true;

                crateBody.isKinematic = false;
                crate.rotate = false;
                thisCrateFruit.Value = 0;
            }
        }

        

        if (releaseCrateCoroutine != null)
            StopCoroutine(releaseCrateCoroutine);

    }

    private IEnumerator ReleaseCrateTimer()
    {
        yield return new WaitForSeconds(releaseCrateTimeoutTime);
        if (cratesList.Count >= 0)
        {
            thisCrateFruit.Value = 0;
            lastCrateOnPile = cratesList[cratesList.Count - 1];
            Rigidbody crateBody = lastCrateOnPile.GetComponent<Rigidbody>();
            CrateBehaviour crate = lastCrateOnPile.GetComponent<CrateBehaviour>();
            crate.crateManager = this;

            ActualCounterSlider.caughtUp = false;

            crateBody.isKinematic = false;
            crate.rotate = false;
        }
    }


    public void DealPoints(float multiplier)
    {

        GameObject textPointsAux = Instantiate(textPoints, pointsTextPos.position, Quaternion.identity, points.transform);
        textPointsAux.GetComponent<Text>().text = "+ " + cratePoints * multiplier;
        pointsVariable.Value += cratePoints * multiplier;
        cratePoints = 0;
    }


    public void DealNegativePoints()
    {
        GameObject textPointsAux = Instantiate(textPoints, pointsTextPos.position, Quaternion.identity, points.transform);
        textPointsAux.GetComponent<Text>().text = "- " + 2;
        pointsVariable.Value -= 2;
        
    }

}
