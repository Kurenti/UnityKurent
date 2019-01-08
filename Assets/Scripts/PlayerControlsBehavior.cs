using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsBehavior : MonoBehaviour {

	public float speed;
	public float rotSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody rb = GetComponent<Rigidbody>();

		if (Input.GetKey(KeyCode.W)) {
			rb.MovePosition(transform.position + transform.forward * speed);
		}

		if (Input.GetKey(KeyCode.S)) {
			rb.MovePosition(transform.position + transform.forward * - speed);
		}

		if (Input.GetKey(KeyCode.A)) {
			Quaternion deltaRotation = Quaternion.Euler(0, - rotSpeed, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);
		}

		if (Input.GetKey(KeyCode.D)) {
			Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed, 0);
			rb.MoveRotation(rb.rotation * deltaRotation);			
		}

		if (Input.GetKey(KeyCode.Alpha1)) {
			
		}

		if (Input.GetKey(KeyCode.Alpha2)) {
			
		}

		if (Input.GetKey(KeyCode.Alpha3)) {
			
		}

		if (Input.GetKey(KeyCode.Alpha4)) {
			
		}
	}
}
