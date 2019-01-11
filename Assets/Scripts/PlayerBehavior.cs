using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private PlayerControls controls;
    private Animator animator;
    private PlayerSnowBehavior psb;

    //attributes
    private float temperature;
    private float stamina;

    [Header("Movement speed")]
    public float maxSpeed;
    public float minSpeed;
    public float rotSpeed;

    private float speed;
    
    [Header("Botany")]
    [Range(0, 1)] public float foliageDensity = 0.75f;
    [Range(1, 5)] public int foliageSpawnRadius = 1;
    public Transform plant1;
    public Transform plant2;
    public Transform plant3;
    public Transform plant4;
    public Transform plant5;
    public Transform plant6;

    private Transform[] plants;
    [HideInInspector] public int currentFoliageSpawnRadius;
    private Texture2D foliageMap;
    private RaycastHit foliageSpawnRay;
    private Vector3 spawnPosition;

    // Use this for initialization
    void Start () {
        //Movement
        //////////
        controls = GetComponent<PlayerControls>();
        animator = GetComponentInChildren<Animator>();
        speed = maxSpeed;

        psb = new PlayerSnowBehavior(null, null);

        //Set up Foliage control
        ////////////////////////
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
        currentFoliageSpawnRadius = foliageSpawnRadius;
        plants = new Transform[]{plant1, plant2, plant3, plant4, plant5, plant6};
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
        }
        //Animate movement speed
        animator.SetFloat("MoveSpeed", controls.moveDirection);

        //Turning left-right
        if (controls.turnDirection != 0) {
            Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * Time.fixedDeltaTime * controls.turnDirection, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        //Jump
        if (controls.jump) {
            animator.SetBool("Jump", true);
            GetComponentInParent<SnowMelter>().currentBrushSize = 2* GetComponentInParent<SnowMelter>().brushSize;
        }


        //Actions
        /////////
        //Attacks
        if (controls.attack1)
        {
            animator.SetBool("Hurricane", true);
            GetComponentInParent<SnowMelter>().currentBrushSize = 3 * GetComponentInParent<SnowMelter>().brushSize;
            currentFoliageSpawnRadius = foliageSpawnRadius + 2;
        }
        if (controls.attack2)
        {
            animator.SetBool("YMCA", true);
            GetComponentInParent<SnowMelter>().currentBrushSize = 4 * GetComponentInParent<SnowMelter>().brushSize;
            currentFoliageSpawnRadius = foliageSpawnRadius + 2;
        }
        if (controls.attack3)
        {
        }
        if (controls.attack4)
        {
        }

        //Gameplay updates
        //////////////////
        ///
        plantFoliage(transform.position);
    }

    public void addBell(GameObject kurent, GameObject bell)
    {
        psb.addBell(kurent, bell);
    }

    private void plantFoliage(Vector3 position)
    {
        //Only plant new foliage if foliageMap at x, y permits it
        float foliageValue = foliageMap.GetPixel((int)position.x, (int)position.z).r;
        if (foliageValue < foliageDensity)
        {
            //Some random foliage generation
            //A pow4 is used as it gives a nice curve turning foliage density to
            //per-frame-spawn-propability at 0->0, 0.5->~0.05, 1.0->1.0
            if (Random.value < Mathf.Pow(foliageDensity, 4)) {
                //Increase red value in the foliage map
                foliageMap.SetPixel((int)position.x, (int)position.z,
                                    new Color(Mathf.Min(
                                        foliageValue + 0.05f,
                                        //1.01 so that even 1 increases foliage count
                                        1.0f),
                                    0.0f,
                                    0.0f));
                
                //Generate random spawn point around player (this can spawn foliage outside of landscape at y zero)
                //SpawnRadius 1 gives 1 square, 2 gives 3*3, 3 gives 5*5...
                spawnPosition = new Vector3((int)position.x - (currentFoliageSpawnRadius - 1) + Random.value * (2*currentFoliageSpawnRadius - 1),
                                            200.0f,
                                            (int)position.z - (currentFoliageSpawnRadius - 1) + Random.value * (2 * currentFoliageSpawnRadius - 1));
                Physics.Raycast(spawnPosition, Vector3.down, out foliageSpawnRay, 200.0f, LayerMask.GetMask("Terrain"));
                spawnPosition.y = foliageSpawnRay.point.y;
                
                //Random plant
                int plantType = Random.Range(0, plants.Length);

                //Random yaw
                Quaternion spawnRotation = Quaternion.Euler(plants[plantType].localEulerAngles.x,
                                                            Random.Range(0, 360),
                                                            plants[plantType].localEulerAngles.z);

                Instantiate(plants[plantType], spawnPosition + plants[plantType].position, spawnRotation);
            }
        }
    }
}
