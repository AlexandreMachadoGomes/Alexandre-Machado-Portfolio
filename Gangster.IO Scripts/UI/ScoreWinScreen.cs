using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWinScreen : MonoBehaviour
{
    public bool firstStar = false;
    public bool secondStar = false;
    public bool thirdStar = false;

    public bool startUpdatingScore = false;

    public bool typingName = false;
    public bool nameTyped = false;
    public GameObject inputName;

    public float totalScore;

    public List<float> starMilestones; 

    public Text scoreText;

    public List<ParticleSystem> firstStartConfetti;
    

    public List<WinScreenTarget> targets;

    public List<FadeOut> scoreCounterTexts;
    public List<FadeInTutorial> highScoreTexts;


    public List<string> userNames;
    public List<int> userScores;
    public InputField inputField;

    public Button playAgainButton;


    // Start is called before the first frame update
    void Start()
    {
        //PlayerPrefs.DeleteAll();

        userScores = new List<int>();
        userNames = new List<string>();

        totalScore = PlayerPrefs.GetInt("thisRunScore");
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
                SubmitName();
            }
        if (nameTyped)
        {
            nameTyped = false;
            ChangeToScore();
            inputName.GetComponent<FadeOut>().readyFadeOut = true;
        }
    }


    public void SubmitName()
    {
        typingName = false;
        nameTyped = true;
        inputField.interactable = false;
    }

    private void UpdateScoreNumbers()
    {
        int newScore = int.Parse(scoreText.text) + 2;
        scoreText.text = (newScore).ToString();
        if (newScore >= starMilestones[0] && !firstStar)
        {
            firstStar = true;
            for (int i = 0; i < firstStartConfetti.Count; i++)
            {
                firstStartConfetti[i].Play();
                
            }
            targets[0].milestoneAcomplished = true;

        }
        else if (newScore >= starMilestones[1] && !secondStar)
        {
            secondStar = true;
            for (int i = 0; i < firstStartConfetti.Count; i++)
            {
                firstStartConfetti[i].Play();
            }
            targets[1].milestoneAcomplished = true;
        }
        else if (newScore >= starMilestones[2] && !thirdStar)
        {
            thirdStar = true;
            for (int i = 0; i < firstStartConfetti.Count; i++)
            {
                firstStartConfetti[i].Play();
            }
            targets[2].milestoneAcomplished = true;
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

        for (int z = 9; z < highScoreTexts.Count - 4; z++)
        {
            if (scoreNumber >= userNames.Count)
            {
                Invoke("FadeInButton", 2);
                return;
            }
            highScoreTexts[z].GetComponent<Text>().text = userNames[scoreNumber] + "   " + userScores[scoreNumber].ToString();
            scoreNumber++;

        }

        for (int z = 13; z < highScoreTexts.Count; z++)
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
        playAgainButton.gameObject.SetActive(true);
        playAgainButton.GetComponent<FadeInButton>().StartFadein();

    }
}

