using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnowBehavior {

	public GameObject leftBell;
	public GameObject rightBell;
	
	private int maxPower;

    public PlayerSnowBehavior (GameObject leftBell, GameObject rightBell) {
		this.leftBell = leftBell;
		this.rightBell = rightBell;
		
		this.maxPower = 0;
	}
	
	public void addBell(GameObject kurent, GameObject bell) {
		
		BellBehavior bb = (BellBehavior) bell.GetComponent(typeof(BellBehavior));
		if (this.maxPower < bb.power) {
			this.maxPower = bb.power;
		}
		
		if (this.leftBell == null) {
			Debug.Log("LEFT");
			this.leftBell = bell;
			bell.transform.parent = kurent.transform;
			bell.transform.localPosition = new Vector3(-2.85f, -2.71f, 2.2f);
			return;
		} else if (this.rightBell == null) {
			Debug.Log("RIGHT");
			this.rightBell = bell;
			bell.transform.parent = kurent.transform;
			bell.transform.localPosition = new Vector3(2.85f, -2.71f, 2.2f);
			return;
		}
	}
	
}
