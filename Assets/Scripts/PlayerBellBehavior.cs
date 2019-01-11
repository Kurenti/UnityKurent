using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBellBehavior : MonoBehaviour
{

	private GameObject leftBell;
    private GameObject rightBell;
	
	[HideInInspector] public int maxPower;
	
	public void AddBell(GameObject bell) {

        //Bell capabilities
        ///////////////////
        if (this.maxPower < bell.GetComponent<BellBehavior>().power)
        {
            this.maxPower = bell.GetComponent<BellBehavior>().power;

            //PlayerStuff
            if (this.maxPower == 2)
            {
                GetComponent<PlayerBehavior>().foliageSpawnRadius *= 2;
                GetComponent<PlayerBehavior>().currentFoliageSpawnRadius = GetComponent<PlayerBehavior>().foliageSpawnRadius;
            }
        }

        //Visuals
        /////////
        if (this.leftBell == null) {
            //Bell stuff
			this.leftBell = bell;
			bell.transform.parent = transform.Find("Kurent").Find("mixamorig:Hips").Find("mixamorig:LeftUpLeg");
            bell.transform.localPosition = new Vector3(0.126f, 0.036f, 0.015f);
            bell.transform.eulerAngles = new Vector3(24.246f, -8.545f, -2.107f);
        }
        else if (this.rightBell == null)
        {
            //Bell stuff
            this.rightBell = bell;
			bell.transform.parent = transform.Find("Kurent").Find("mixamorig:Hips").Find("mixamorig:RightUpLeg");
            bell.transform.localPosition = new Vector3(-0.138f, 0.121f, -0.014f);
            bell.transform.eulerAngles = new Vector3(40.0f, 30.0f, 20);
		}

        //Audio trigger
        ///////////////
        if (Random.Range(0, 1) < 0.5)
            GetComponentInChildren<AudioBehavior>().PlayBell1();
        else
            GetComponentInChildren<AudioBehavior>().PlayBell2();

    }
}
