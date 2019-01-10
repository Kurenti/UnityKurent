using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private PlayerControls controls;
    private Animator animator;
    private PlayerSnowBehavior psb;
	private AudioBehavior kurentAudio;

    [Header("Movement speed")]
    public float groundSpeed;
    public float snowSpeed;
    public float rotSpeed;

    private float speed;
    private float animSpeed;
    private float animStartSpeed;
    
    [Header("Botany")]
    [Range(0, 1)] public float foliageDensity = 0.75f;
    public Transform plant1;

    [HideInInspector] public int foliageSpawnRadius = 1;
    private Texture2D foliageMap;
    private RaycastHit foliageSpawnHeight;

    // Use this for initialization
    void Start () {
        controls = GetComponent<PlayerControls>();
        animator = GetComponentInChildren<Animator>();
        psb = new PlayerSnowBehavior(null, null);
		kurentAudio = GetComponentInChildren<AudioBehavior>();

        speed = groundSpeed;
        //This is currently unused, further testing needed to see if actually improves gameplay
        animStartSpeed = 0.2f;

        //Set up foliageMap
        //Foliage map relies on landscape having a corner in 0,y,0 and expanding in +x, +z
        var allObjects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == LayerMask.NameToLayer("Terrain"))
            {
                foliageMap = new Texture2D((int)Mathf.Floor(obj.GetComponent<Renderer>().bounds.size.x),
                                           (int)Mathf.Floor(obj.GetComponent<Renderer>().bounds.size.z));
                Color blackCol = new Color(0, 0, 0);
                var texArray = foliageMap.GetPixels();
                for (var i = 0; i < texArray.Length; i++)
                {
                    texArray[i] = blackCol;
                }
                foliageMap.SetPixels(texArray);
                break;
            }
        }
    }

    // FixedUpdate is called once per physics update
    void FixedUpdate () {
        Rigidbody rb = GetComponent<Rigidbody>();

        //Movement
        //////////
        
        //Forward-backward movement
        if (controls.moveDirection != 0) {

            //Move position
            rb.MovePosition(transform.position +
                            transform.forward * speed * Time.fixedDeltaTime * controls.moveDirection);
            plantFoliage(transform.position);
            
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
			
			// Play steps sound
			kurentAudio.PlaySteps();

        } else {
            animSpeed = animSpeed == 0 ? (animSpeed) :
                            (animSpeed > 0 ? (Mathf.Max(0, animSpeed - animStartSpeed)) :
                                             (Mathf.Min(0, animSpeed + animStartSpeed)));
			 
			// Stop steps sound
			kurentAudio.StopSteps();
        }

        //This line ommits the above animSpeed stuff
        animSpeed = controls.moveDirection;

        //Animate movement speed
        animator.SetFloat("MoveSpeed", animSpeed);


        //Turning left-right
        if (controls.turnDirection != 0) {
            Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * Time.fixedDeltaTime * controls.turnDirection, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        //Jump
        if (controls.jump) {
            animator.SetBool("Jump", true);
            GetComponentInParent<SnowMelter>().currentBrushSize = 2* GetComponentInParent<SnowMelter>().brushSize;
			
			if (controls.moveDirection != 0) {
				kurentAudio.PlayJump(0f);
			} else {
				kurentAudio.PlayJump(1f);
			}
			
        }


        //Actions
        /////////

        //Attacks
        if (controls.attack1)
        {
            animator.SetBool("Hurricane", true);
            GetComponentInParent<SnowMelter>().currentBrushSize = 3 * GetComponentInParent<SnowMelter>().brushSize;
			
			kurentAudio.PlayAttack1();
        }

        if (controls.attack2)
        {
            animator.SetBool("YMCA", true);
            GetComponentInParent<SnowMelter>().currentBrushSize = 4 * GetComponentInParent<SnowMelter>().brushSize;
			
			kurentAudio.PlayAttack2();
        }

        if (controls.attack3)
        {
        }

        if (controls.attack4)
        {
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

    private void plantFoliage(Vector3 position)
    {
        //Only plant new foliage if foliageMap at x, y permits it
        float foliageValue = foliageMap.GetPixel((int)position.x, (int)position.z).r;
        if (foliageValue < 1.0f)
        {
            //Some random foliage generation
            //A pow4 is used as it gives a nice curve turning foliage density to
            //per-frame-spawn-propability at 0->0, 0.5->~0.05, 1.0->1.0
            if (Random.value < Mathf.Pow(foliageDensity, 4)) {
                //Increase red value in the foliage map
                foliageMap.SetPixel((int)position.x, (int)position.z,
                                    new Color(Mathf.Min(
                                        foliageValue + (1.01f - foliageDensity),
                                        //1.01 so that even 1 increases foliage count
                                        1.0f),
                                    0.0f,
                                    0.0f));
                
                //Generate random spawn point around player (this can spawn foliage outside of landscape at y zero)
                Vector3 spawnPosition = new Vector3((int)position.x + Random.value * foliageSpawnRadius,
                                                    200.0f,
                                                    (int)position.z + Random.value * foliageSpawnRadius);
                Physics.Raycast(spawnPosition, Vector3.down, out foliageSpawnHeight, 200.0f, LayerMask.GetMask("Terrain"));
                spawnPosition.y = foliageSpawnHeight.point.y;

                //Generate plant
                Instantiate(plant1, spawnPosition, Quaternion.identity);
            }
        }
    }
}
