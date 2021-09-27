using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Animator killTextAnim;
    public Image medalImg;
    public Sprite[] rend;
    public Text mdealText;
    public string[] appreciationTexts;
    public Transform killTextDesiredPos, killTextObj;

    public int kills;
    public Text killText;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    public void OnRestart()
    {
        SceneManager.LoadScene(0);
    }

    public void PopKillTextEffect()
    {
        medalImg.sprite = rend[Random.Range(0, rend.Length)];
        mdealText.text = appreciationTexts[Random.Range(0, appreciationTexts.Length)];
        killTextAnim.Play("ktpop", -1, 0.0f);
        kills++;
        killText.text = kills.ToString("0");
    }

    private void Update()
    {
        killTextObj.position = killTextDesiredPos.position;
    }
}