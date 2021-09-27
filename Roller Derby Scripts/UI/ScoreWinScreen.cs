using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWinScreen : MonoBehaviour
{
    public bool firstStar = false;
    public bool secondStar = false;
    public bool thirdStar = false;
    public Material starYellow;

    public bool startUpdatingScore = false;

    public bool typingName = false;
    public bool nameTyped = false;
    public GameObject inputName;

    public float totalScore;

    public List<float> starMilestones;

    public Text scoreText;

    public List<FadeInTutorial> highScoreTexts;
    public List<FadeOut> scoreCounterTexts;

    public List<ParticleSystem> firstStartConfetti;
    public List<ParticleSystem> secondStartConfetti;
    public List<ParticleSystem> thirdStartConfetti;

    public List<MeshRenderer> stars;

    public List<string> userNames;
    public List<int> userScores;
    public InputField inputField;

    public Button playAgainButton;

    // Start is called before the first frame update
    void Start()
    {
        userScores = new List<int>();
        userNames = new List<string>();

        //totalScore = PlayerPrefs.GetInt("thisRunScore");
        PlayerPrefs.DeleteKey("thisRunScore");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (startUpdatingScore)
            UpdateScoreNumbers();
        if (typingName)
            if (Input.GetButtonDown("Submit"))
            {
                typingName = false;
                nameTyped = true;
                inputField.interactable = false;
            }
        if (nameTyped)
        {
            nameTyped = false;
            ChangeToScore();
            inputName.GetComponent<FadeOut>().readyFadeOut = true;
        }
    }


    private void UpdateScoreNumbers()
    {
        int newScore = int.Parse(scoreText.text) + 25;
        scoreText.text = (newScore).ToString();
        if (newScore >= starMilestones[0] && !firstStar)
        {
            firstStar = true;
            stars[0].material = starYellow;
            for (int i = 0; i < firstStartConfetti.Count; i++)
            {
                ParticleSystem.MainModule star1 = firstStartConfetti[i].main;
                star1.duration = .75f;
                firstStartConfetti[i].Play();
            }

        }
        else if (newScore >= starMilestones[1] && !secondStar)
        {
            secondStar = true;
            stars[1].material = starYellow;
            for (int i = 0; i < secondStartConfetti.Count; i++)
            {
                ParticleSystem.MainModule star1 = firstStartConfetti[i].main;

                star1.duration = 1;
                firstStartConfetti[i].Play();
                ParticleSystem.MainModule star2 = secondStartConfetti[i].main;
                star2.duration = 1;
                secondStartConfetti[i].Play();
            }
        }
        else if (newScore >= starMilestones[2] && !thirdStar)
        {
            thirdStar = true;
            stars[2].material = starYellow;
            for (int i = 0; i < thirdStartConfetti.Count; i++)
            {
                ParticleSystem.MainModule star1 = firstStartConfetti[i].main;
                star1.duration = 1.5f;
                ParticleSystem.MainModule star2 = secondStartConfetti[i].main;
                star2.duration = 1.5f;
                ParticleSystem.MainModule star3 = thirdStartConfetti[i].main;
                star3.duration = 1.5f;
                firstStartConfetti[i].Play();
                secondStartConfetti[i].Play();
                thirdStartConfetti[i].Play();
            }
        }
        if (newScore >= totalScore)
        {
            startUpdatingScore = false;
            TypeYourName();
        }
    }

    private void ChangeToScore()
    {
        for (int i = 0; i < scoreCounterTexts.Count; i++)
        {
            scoreCounterTexts[i].readyFadeOut = true;
        }

        int highscoreUsersNumber = 0;
        if (PlayerPrefs.GetInt("highscoreUsersNumber") != null)
            highscoreUsersNumber = PlayerPrefs.GetInt("highscoreUsersNumber");

        highscoreUsersNumber += 1;
        PlayerPrefs.SetInt("highscoreUsersNumber", highscoreUsersNumber);

        PlayerPrefs.SetInt("UserScore" + highscoreUsersNumber.ToString(), int.Parse(GameObject.Find("Score").GetComponent<Text>().text)); ;
        PlayerPrefs.SetString("UserName" + highscoreUsersNumber.ToString(), inputField.text.ToString());

        Invoke("ChangeToScoreContinued", 2);
    }

    private void TypeYourName()
    {
        typingName = true;
        inputName.SetActive(true);


    }


    private void ChangeToScoreContinued()
    {
        for (int i = 0; i < highScoreTexts.Count; i++)
        {
            highScoreTexts[i].fadeIn = true;
        }

        for (int i = 0; i < PlayerPrefs.GetInt("highscoreUsersNumber"); i++)
        {

            bool inserted = false;
            for (int n = 0; n < i; n++)
            {

                if (PlayerPrefs.GetInt("UserScore" + (i + 1)) > userScores[n])
                {

                    userScores.Insert(n, PlayerPrefs.GetInt("UserScore" + (i + 1)));
                    userNames.Insert(n, PlayerPrefs.GetString("UserName" + (i + 1)));
                    inserted = true;
                    break;

                }


            }
            if (!inserted)
            {
                int a = PlayerPrefs.GetInt("UserScore" + (i + 1));
                userScores.Add(a);
                userNames.Add(PlayerPrefs.GetString("UserName" + (i + 1)));
                inserted = false;
            }

            if (PlayerPrefs.GetInt("highscoreUsersNumber") > 10)
            {
                PlayerPrefs.DeleteKey("UserScore" + 11);
            }

        }

        int scoreNumber = 0;

        for (int z = 11; z < highScoreTexts.Count - 5; z++)
        {
            if (scoreNumber >= userNames.Count)
            {
                Invoke("FadeInButton", 2);
                return;
            }
            highScoreTexts[z].GetComponent<Text>().text = userNames[scoreNumber] + "   " + userScores[scoreNumber].ToString();
            scoreNumber++;

        }

        for (int z = 16; z < highScoreTexts.Count; z++)
        {
            if (scoreNumber >= userNames.Count)
            {
                Invoke("FadeInButton", 2);
                return;
            }
            highScoreTexts[z].GetComponent<Text>().text = userScores[scoreNumber] + "   " + userNames[scoreNumber].ToString();
            scoreNumber++;
        }

        Invoke("FadeInButton", 2);

    }

    

    private void FadeInButton()
    {
        playAgainButton.GetComponent<FadeInButton>().StartFadein();

    }
}


