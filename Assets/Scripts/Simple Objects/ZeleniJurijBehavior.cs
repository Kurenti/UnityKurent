using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeleniJurijBehavior : MonoBehaviour {

    private bool playerClose;
    private bool jurijEventStarted;

    private GameObject player;
    private Transform bell;
    public float bellRiseSpeed = 0.5f;
    private float bellRemainingOffset;

    // Use this for initialization
    void Start () {
        jurijEventStarted = false;
        playerClose = false;

        bell = transform.parent.Find("JurijBell");
        bellRemainingOffset = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
        if (!jurijEventStarted && playerClose)
        {
            if (player.GetComponent<PlayerFoliage>().GetEnvironmentTemperature(transform.position) > 0.2f)
            {
                jurijEventStarted = true;
                player.GetComponent<PlayerBehavior>().cleanJurij = true;
                iTween.MoveTo(gameObject, new Vector3(129.4f, 41.93f, 244.67f), 20);
                iTween.RotateTo(gameObject, new Vector3(97.37f, 188.81f, -5.15f), 20);
            }
        }

		if (jurijEventStarted)
        {
            if (bellRemainingOffset > 0.0f)
            {
                bell.position += bell.forward * bellRiseSpeed * Time.deltaTime;
                bellRemainingOffset -= bellRiseSpeed * Time.deltaTime;
            }
            else
                this.enabled = false;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerObject" && !jurijEventStarted)
        {
            player = other.gameObject;
            other.GetComponent<PlayerBehavior>().nearJurij = true;
            playerClose = true;
        }
    }
}
