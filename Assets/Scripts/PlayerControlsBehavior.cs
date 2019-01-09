using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlsBehavior : MonoBehaviour {

    //Animations and movement to be moved out of here ffs

    public float groundSpeed;
    public float snowSpeed;
	public float rotSpeed;

    private float speed;
    private float animSpeed;

    private Animator animator;
	private PlayerSnowBehavior psb;
	
	// Use this for initialization
	void Start () {
        animator = GetComponentInChildren<Animator>();
		psb = new PlayerSnowBehavior(null, null);

        speed = groundSpeed;
	}

    // Update is called once per frame
    void Update() {
        Rigidbody rb = GetComponent<Rigidbody>();

        //Forward-backward movement
        if (Input.GetKey(KeyCode.W)) {
            rb.MovePosition(transform.position + transform.forward * speed);
            animSpeed += 0.1f;
            if (animSpeed > speed / groundSpeed) {
                //Case just limit animSpeed to max 1 / 0.5
                if (animSpeed - speed / groundSpeed <= 0.2)
                    animSpeed = speed / groundSpeed;
                //Case decay animSpeed from running to walking
                else
                    animSpeed -= 0.2f;
            }

        }
        else if (Input.GetKey(KeyCode.S)) {
            rb.MovePosition(transform.position + transform.forward * -speed);
            animSpeed -= 0.1f;
            if (animSpeed < -1)
                animSpeed = -1;
        }
        else {
            //Decay animation speed
            if (animSpeed > 0) {
                animSpeed -= 0.1f;
                if (animSpeed < 0)
                    animSpeed = 0;
            }
            else if (animSpeed < 0) {
                animSpeed += 0.1f;
                if (animSpeed > 0)
                    animSpeed = 0;
            }
        }

        animator.SetFloat("MoveSpeed", animSpeed);

        //Turning left-right
        if (Input.GetKey(KeyCode.A)) {
            Quaternion deltaRotation = Quaternion.Euler(0, -rotSpeed, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        if (Input.GetKey(KeyCode.D)) {
            Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        //Jump
        if (Input.GetKey(KeyCode.Space)) {
            animator.SetBool("Jump", true);
        }

        //Attacks
		if (Input.GetKey(KeyCode.Alpha1)) {
			psb.Attack(1);
            animator.SetBool("Hurricane", true);
		}

		if (Input.GetKey(KeyCode.Alpha2)) {
			psb.Attack(2);
		}

		if (Input.GetKey(KeyCode.Alpha3)) {
			psb.Attack(3);
		}

		if (Input.GetKey(KeyCode.Alpha4)) {
			psb.Attack(4);
		}
	}
	
	public void addBell(GameObject kurent, GameObject bell) {
		psb.addBell(kurent, bell);
	}

    public void setGroundSpeed()
    {
        if (speed != groundSpeed)
            speed = groundSpeed;
    }

    public void setSnowSpeed()
    {
        if (speed != snowSpeed)
            speed = snowSpeed;
    }
}
