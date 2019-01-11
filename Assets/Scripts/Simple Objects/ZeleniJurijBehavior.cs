using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeleniJurijBehavior : MonoBehaviour {

    private bool jurijEventStarted;
    
    public float jurijRiseSpeed = 1.0f;
    private float jurijRemainingOffset;

    // Use this for initialization
    void Start () {
        jurijEventStarted = false;

        jurijRemainingOffset = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {
		if (jurijEventStarted)
        {
            if (jurijRemainingOffset > 0.0f)
            {
                transform.position -= transform.forward * jurijRiseSpeed * Time.deltaTime;
                jurijRemainingOffset -= jurijRiseSpeed * Time.deltaTime;
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "PlayerObject")
        {
            jurijEventStarted = true;
        }
    }
}
