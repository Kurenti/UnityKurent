using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private PlayerControls controls;
    private Animator animator;
    private PlayerSnowBehavior psb;

    public float groundSpeed;
    public float snowSpeed;
    public float rotSpeed;

    private float speed;
    private float animSpeed;
    private float animStartSpeed;

    // Use this for initialization
    void Start () {
        controls = GetComponent<PlayerControls>();
        animator = GetComponentInChildren<Animator>();
        psb = new PlayerSnowBehavior(null, null);

        speed = groundSpeed;
        //This is currently unused, further testing needed to see if actually improves gameplay
        animStartSpeed = 0.2f;
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate () {
        Rigidbody rb = GetComponent<Rigidbody>();

        //Movement
        //////////
        
        //Forward-backward movement
        if (controls.moveDirection != 0) {

            //Move position
            //-------needs timedelta!!!!------------
            rb.MovePosition(transform.position + transform.forward * speed * controls.moveDirection);
            
            //Everything from here till setting animator "MoveSpeed" is currently not used, testing
            //needed to see if it is actually needed
            //Increase animation move speed
            animSpeed += animStartSpeed * controls.moveDirection;

            //Positive direction speed limiting
            if (animSpeed > speed / groundSpeed)
            {
                //Case: just limit animSpeed to max 1 or 0.5
                if (animSpeed - speed / groundSpeed <= animStartSpeed*2)
                    animSpeed = speed / groundSpeed;
                //Case: decay animSpeed from running to walking
                else
                    animSpeed -= animStartSpeed*2;
            }
            //Negative direction speed limiting
            else if (animSpeed < -1)
                animSpeed = -1;

        } else {
            animSpeed = animSpeed == 0 ? (animSpeed) :
                            (animSpeed > 0 ? (Mathf.Max(0, animSpeed - animStartSpeed)) :
                                             (Mathf.Min(0, animSpeed + animStartSpeed)));
        }

        //This line ommits the above animSpeed stuff
        animSpeed = controls.moveDirection;

        //Animate movement speed
        animator.SetFloat("MoveSpeed", animSpeed);

        //Turning left-right
        if (controls.turnDirection != 0) {
            //-------needs timedelta!!!!------------
            Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * controls.turnDirection, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        //Jump
        if (controls.jump) {
            animator.SetBool("Jump", true);
            //erase this as soon as possible lord forgive me
            var allObjects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in allObjects)
            {
                if (go.layer == 9)
                {
                    go.GetComponent<SnowMelter>()._brushSize = 0.2f;
                    break;
                }
            }
        }

        //Actions
        /////////

        //Attacks
        if (controls.attack1)
        {
            psb.Attack(1);
            animator.SetBool("Hurricane", true);
            //erase this as soon as possible lord forgive me
            var allObjects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in allObjects)
            {
                if (go.layer == 9)
                {
                    go.GetComponent<SnowMelter>()._brushSize = 0.3f;
                    break;
                }
            }
        }

        if (controls.attack1)
        {
            psb.Attack(2);
            animator.SetBool("YMCA", true);
            //erase this as soon as possible lord forgive me
            var allObjects = FindObjectsOfType(typeof(GameObject));
            foreach (GameObject go in allObjects)
            {
                if (go.layer == 9)
                {
                    go.GetComponent<SnowMelter>()._brushSize = 0.4f;
                    break;
                }
            }
        }

        if (controls.attack1)
        {
            psb.Attack(3);
        }

        if (controls.attack1)
        {
            psb.Attack(4);
        }
    }

    public void addBell(GameObject kurent, GameObject bell)
    {
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
