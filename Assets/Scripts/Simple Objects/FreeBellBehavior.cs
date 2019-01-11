using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeBellBehavior : MonoBehaviour {

    private bool playerNear;
    private GameObject player;

    [Range(1.0f, 5.0f)] public float scaleUp = 3.0f;

	// Use this for initialization
	void Start () {
        playerNear = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (playerNear && player.GetComponent<PlayerControls>().interact)
        {
            GetComponent<Transform>().localScale /= scaleUp;

            player.GetComponent<PlayerBellBehavior>().AddBell(gameObject);
            this.enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerObject")
        {
            playerNear = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "PlayerObject")
        {
            playerNear = false;
        }
    }

    private void OnEnable()
    {
        GetComponent<Transform>().localScale *= scaleUp;
    }
}
