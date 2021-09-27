using UnityEngine;

public class ClickPhaseArea : MonoBehaviour
{
    //public GameObject scoreNumbers;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            Player player = other.GetComponent<Player>();
            if (!player.isJumping)
            {
                player.clickingPhase = true;
                player.sliderColor.gameObject.SetActive(true);
                StartCoroutine(player.KickAnimStartTimer());
                //for (int i = 0; i < scoreNumbers.transform.childCount; i++)
                //{
                //    scoreNumbers.transform.GetChild(i).gameObject.SetActive(true);
                //}
            }
        }
        else if (other.CompareTag("PlayerBall"))
            Destroy(other);
    }
}