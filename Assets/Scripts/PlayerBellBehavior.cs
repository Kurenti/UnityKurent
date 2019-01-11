﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBellBehavior : MonoBehaviour
{

	private GameObject leftBell;
    private GameObject rightBell;
	
	private int maxPower;
	
	public void AddBell(GameObject bell) {

        if (Random.Range(0, 1) < 0.5)
            GetComponentInChildren<AudioBehavior>().PlayBell1();
        else
            GetComponentInChildren<AudioBehavior>().PlayBell2();

        BellBehavior bb = (BellBehavior) bell.GetComponent(typeof(BellBehavior));
		if (this.maxPower < bb.power) {
			this.maxPower = bb.power;
		}
		
		if (this.leftBell == null) {
			this.leftBell = bell;
			bell.transform.parent = transform.Find("Kurent").Find("mixamorig:Hips").Find("mixamorig:LeftUpLeg");
            bell.transform.localPosition = new Vector3(0.126f, 0.036f, 0.015f);
            bell.transform.eulerAngles = new Vector3(24.246f, -8.545f, -2.107f);
            return;
		} else if (this.rightBell == null) {
			this.rightBell = bell;
			bell.transform.parent = transform.Find("Kurent").Find("mixamorig:Hips").Find("mixamorig:RightUpLeg");
            bell.transform.localPosition = new Vector3(-0.138f, 0.121f, -0.014f);
            bell.transform.eulerAngles = new Vector3(40.0f, 30.0f, 20);
            return;
		}
	}
	
}
