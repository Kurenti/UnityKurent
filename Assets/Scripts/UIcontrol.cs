using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour {

    public GameObject player;

    private GameObject minimap;

	// Use this for initialization
	void Start () {
        minimap = transform.Find("Minimap").gameObject;

    }
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(0).GetComponent<Slider>().value =
            player.GetComponent<PlayerBehavior>().stamina;
        transform.GetChild(1).GetComponent<Slider>().value =
            player.GetComponent<PlayerBehavior>().temperature;

        //Minimap
        if (player.GetComponent<PlayerControls>().minimap && !minimap.activeInHierarchy)
            minimap.SetActive(true);
        else if (!player.GetComponent<PlayerControls>().minimap && minimap.activeInHierarchy)
            minimap.SetActive(false);

        //DeathScreen
        if (player.GetComponent<PlayerBehavior>().dead)
        {
            player.GetComponent<PlayerBehavior>().dead = false;
            transform.Find("StartMenuCanvas").gameObject.SetActive(true);
            GetComponentInChildren<MenuBehavior>().Died();
        }

        //Pause
        if (player.GetComponent<PlayerControls>().pause)
        {
            transform.Find("StartMenuCanvas").gameObject.SetActive(true);
            GetComponentInChildren<MenuBehavior>().Pause();
        }
    }
}
