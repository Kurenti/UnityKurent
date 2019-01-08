using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickBehavior : MonoBehaviour {
	
	public string appendingBell;
		
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerEnter(Collider other) {
        
		GameObject bell = GameObject.Find(appendingBell);
		GameObject kurent = GameObject.Find("PlayerObject");

		PlayerControlsBehavior pcb = (PlayerControlsBehavior) kurent.GetComponent(typeof(PlayerControlsBehavior));
		pcb.addBell(kurent, bell);
    }

}