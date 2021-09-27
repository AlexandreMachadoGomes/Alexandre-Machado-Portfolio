using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeTrunk : MonoBehaviour
{

    public int life;
    public GameObject shotEffect;
    public GameObject player;
    public GameObject slider;
    private Slider sliderComponent;

    // Start is called before the first frame update
    void Start()
    {
        sliderComponent = slider.GetComponent<Slider>();
    }

    // Update is called once per frame 
    void Update()
    {
        sliderComponent.value = life;
    }

    public void LooseLife()
    {
        shotEffect.GetComponent<ParticleSystem>().Play();
        this.life -= 1;
        if (life == 0)
        {
            DestroyTree();
        }
    }

    private void DestroyTree()
    {
        Transform person = this.transform.GetChild(0).transform.GetChild(0);
        Quaternion worldRotation = transform.rotation * person.rotation;
        person.SetParent(null);
        person.GetComponent<Hostage>().BrokeFree();
        person.transform.rotation = worldRotation;
        player.GetComponent<Player>().TreeDestroyed();
        Destroy(slider);
        Destroy(this.gameObject);
    }

    public void PrepareTarget()
    {
        slider.gameObject.SetActive(true);
    }

}
