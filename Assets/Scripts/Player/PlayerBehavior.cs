using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private PlayerControls controls;
    private Animator animator;
    private AudioBehavior kurentAudio;

    //attributes
    [HideInInspector] public float temperature;
    [HideInInspector] public float stamina;

    [Header("Movement speed")]
    public float maxSpeed;
    public float minSpeed;
    public float rotSpeed;
    private float speed;

    //Temp for quick release
    [HideInInspector] public bool nearJurij;
    [HideInInspector] public bool cleanJurij;
    [HideInInspector] public bool dead;

    // Use this for initialization
    void Start () {
        //Movement
        //////////
        controls = GetComponent<PlayerControls>();
        animator = GetComponentInChildren<Animator>();
        speed = maxSpeed;
        
        kurentAudio = GetComponentInChildren<AudioBehavior>();

        //Gameplay atributes
        ////////////////////
        temperature = 0.5f;
        stamina = 0.5f;

        ///
        nearJurij = false;
        dead = false;
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate () {

        //Dealing with controlls
        ////////////////////////
        ///
        Rigidbody rb = GetComponent<Rigidbody>();
        //Movement
        //////////
        //Forward-backward movement
        if (controls.moveDirection != 0) {
            rb.MovePosition(transform.position +
                            transform.forward * speed * Time.fixedDeltaTime * controls.moveDirection);
            kurentAudio.PlaySteps();
        } else {
            kurentAudio.StopSteps();
        }
        //Animate movement speed
        animator.SetFloat("MoveSpeed", (speed/maxSpeed) * controls.moveDirection);
        //Limit to terrain
        if (!GetComponent<PlayerFoliage>().teren.GetComponent<Collider>().bounds.Contains(transform.position))
            rb.MovePosition(transform.position -
                            transform.forward * speed * Time.fixedDeltaTime * controls.moveDirection);

        //Turning left-right
        if (controls.turnDirection != 0) {
            Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * Time.fixedDeltaTime * controls.turnDirection, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        //Jump
        if (controls.jump && !animator.GetBool("Jump")) {
            temperature += 0.14f;
            GetComponentInParent<SnowMelter>().currentBrushSize = 2* GetComponentInParent<SnowMelter>().brushSize;
            GetComponent<PlayerFoliage>().PlantFoliage(1);

            animator.SetBool("Jump", true);
            if (controls.moveDirection != 0) {
                kurentAudio.PlayJump(0f);
            } else {
                kurentAudio.PlayJump(1f);
            }
        }

        //Interact
        if (controls.interact && !animator.GetBool("Interact"))
            animator.SetBool("Interact", true);
        
        //Actions
        /////////
        //Attacks
        if (controls.attack1 &&
            GetComponent<PlayerBellBehavior>().maxPower >= 1 &&
            !animator.GetBool("Hurricane"))
        {
            if (stamina < 0.2f)
            {
                NotEnoughStamina();
            } else {
                stamina -= 0.2f;
                temperature += 0.2f;
                GetComponentInParent<SnowMelter>().currentBrushSize = 3 * GetComponentInParent<SnowMelter>().brushSize;
                GetComponent<PlayerFoliage>().currentFoliageSpawnRadius = GetComponent<PlayerFoliage>().foliageSpawnRadius + 1;
                GetComponent<PlayerFoliage>().PlantFoliage(2);

                animator.SetBool("Hurricane", true);
                kurentAudio.PlayAttack1();
            }
        }
        if (controls.attack2 &&
            GetComponent<PlayerBellBehavior>().maxPower >= 2 &&
            !animator.GetBool("YMCA"))
        {
            if (stamina < 0.3f)
            {
                NotEnoughStamina();
            } else {
                stamina -= 0.3f;
                temperature += 0.3f;
                GetComponentInParent<SnowMelter>().currentBrushSize = 4 * GetComponentInParent<SnowMelter>().brushSize;
                GetComponent<PlayerFoliage>().currentFoliageSpawnRadius = GetComponent<PlayerFoliage>().foliageSpawnRadius + 2;
                GetComponent<PlayerFoliage>().PlantFoliage(3);

                animator.SetBool("YMCA", true);
                kurentAudio.PlayAttack2();
            }
        }
        if (controls.attack3 &&
            GetComponent<PlayerBellBehavior>().maxPower >= 3)
        {
            kurentAudio.PlayAttack3();
        }
        if (controls.attack4 &&
            GetComponent<PlayerBellBehavior>().maxPower >= 4)
        {
            kurentAudio.PlayAttack4();
        }

        //Gameplay updates
        //////////////////
        ///
        //Atributes update
        stamina += 0.05f * Time.fixedDeltaTime;
        stamina = Mathf.Clamp(stamina, 0.0f, 1.0f);
        temperature -= ((0.9f - GetComponent<PlayerFoliage>().GetEnvironmentTemperature(transform.position)) / 0.9f) *
                        0.05f * Time.fixedDeltaTime;
        temperature = Mathf.Clamp(temperature, 0.0f, 1.0f);
        //Update move speed
        speed = minSpeed + temperature * (maxSpeed - minSpeed);

        //Try to plant foliage
        GetComponent<PlayerFoliage>().PlantFoliage(0);
    }

    private void NotEnoughStamina()
    {

    }
}
