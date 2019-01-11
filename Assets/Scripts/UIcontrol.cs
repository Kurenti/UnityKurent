using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIcontrol : MonoBehaviour {

    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(0).GetComponent<Slider>().value =
            player.GetComponent<PlayerBehavior>().stamina;
        transform.GetChild(1).GetComponent<Slider>().value =
            player.GetComponent<PlayerBehavior>().temperature;
    }
}
