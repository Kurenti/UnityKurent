using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    private PlayerControls controls;
    private Animator animator;
    private AudioBehavior kurentAudio;

    //attributes
    private float envTemperature;
    [HideInInspector] public float temperature;
    [HideInInspector] public float stamina;

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
    [HideInInspector] public float currentFoliageDensity;
    [HideInInspector] public int currentFoliageSpawnRadius;
    private Terrain teren;
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
        
        kurentAudio = GetComponentInChildren<AudioBehavior>();

        //Gameplay atributes
        ////////////////////
        temperature = 0.5f;
        stamina = 0.5f;

        //Set up Foliage control
        ////////////////////////
        teren = Terrain.activeTerrain;

        foliageMap = new Texture2D((int)Mathf.Floor(teren.terrainData.size.x),
                                   (int)Mathf.Floor(teren.terrainData.size.z));

        Color blackCol = new Color(0, 0, 0);
        Color[] texArray = foliageMap.GetPixels();
        for (var i = 0; i < texArray.Length; i++) {
            texArray[i] = blackCol;
        }
        foliageMap.SetPixels(texArray);

        currentFoliageSpawnRadius = foliageSpawnRadius;
        currentFoliageDensity = foliageDensity;
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
            kurentAudio.PlaySteps();
        } else {
            kurentAudio.StopSteps();
        }
        //Animate movement speed
        animator.SetFloat("MoveSpeed", (speed/maxSpeed) * controls.moveDirection);

        //Turning left-right
        if (controls.turnDirection != 0) {
            Quaternion deltaRotation = Quaternion.Euler(0, rotSpeed * Time.fixedDeltaTime * controls.turnDirection, 0);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }

        //Jump
        if (controls.jump && !animator.GetBool("Jump")) {
            temperature += 0.14f;
            GetComponentInParent<SnowMelter>().currentBrushSize = 2* GetComponentInParent<SnowMelter>().brushSize;
            currentFoliageDensity = foliageDensity + 0.3f * (1.0f - foliageDensity);
            currentFoliageSpawnRadius = foliageSpawnRadius + 1;

            animator.SetBool("Jump", true);
            if (controls.moveDirection != 0) {
                kurentAudio.PlayJump(0f);
            } else {
                kurentAudio.PlayJump(1f);
            }
        }


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
                currentFoliageDensity = foliageDensity + 0.5f * (1.0f - foliageDensity);
                currentFoliageSpawnRadius = foliageSpawnRadius + 3;

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
                currentFoliageDensity = 1.0f;
                currentFoliageSpawnRadius = foliageSpawnRadius + 3;

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
        //Get environment temperature as avg snow count in an 11x11 block around player
        Color[] surroundingSnow = foliageMap.GetPixels(
            Mathf.Min(Mathf.Max((int)transform.position.x - 5, 0),
                      (int)teren.terrainData.bounds.max.x - 11),
            Mathf.Min(Mathf.Max((int)transform.position.z - 5, 0),
                      (int)teren.terrainData.bounds.max.z - 11),
            11, 11);

        envTemperature = 0.0f;
        for (var i = 0; i < surroundingSnow.Length; i++)
        {
            envTemperature += surroundingSnow[i].r;
        }
        envTemperature /= (surroundingSnow.Length * foliageDensity);
        //Mathematically envTemp goes up to 1.0. But 0.05 is a more realistic number with
        //how it's calculated and how foliageMap is drawn on
        envTemperature = Mathf.Clamp(envTemperature, 0.0f, 0.05f);

        //Atributes update
        stamina += 0.05f * Time.fixedDeltaTime;
        stamina = Mathf.Clamp(stamina, 0.0f, 1.0f);
        temperature -= ((0.05f - envTemperature) / 0.05f) * 0.05f * Time.fixedDeltaTime;
        temperature = Mathf.Clamp(temperature, 0.0f, 1.0f);
        //Update move speed
        speed = minSpeed + temperature * (maxSpeed - minSpeed);

        //Try to plant foliage
        PlantFoliage(transform.position);
    }

    private void PlantFoliage(Vector3 position)
    {
        //Only plant new foliage if foliageMap at x, y permits it
        float foliageValue = foliageMap.GetPixel((int)position.x, (int)position.z).r;
        if (foliageValue < currentFoliageDensity)
        {
            //Some random foliage generation
            //A pow4 is used as it gives a nice curve turning foliage density to
            //per-frame-spawn-propability at 0->0, 0.5->~0.05, 1.0->1.0
            if (Random.value < Mathf.Pow(currentFoliageDensity, 4)) {
                //Increase red value in the foliage map
                foliageMap.SetPixel((int)position.x, (int)position.z,
                                    new Color(Mathf.Min(
                                        foliageValue + 0.005f,
                                        1.0f),
                                    0.0f,
                                    0.0f));
                
                //Generate random spawn point around player (this can spawn foliage outside of landscape at y zero)
                //SpawnRadius 1 gives 1 square, 2 gives 3*3, 3 gives 5*5...
                spawnPosition = new Vector3((int)position.x - (currentFoliageSpawnRadius - 1) + Random.value * (2*currentFoliageSpawnRadius - 1),
                                            0.0f,
                                            (int)position.z - (currentFoliageSpawnRadius - 1) + Random.value * (2 * currentFoliageSpawnRadius - 1));
                spawnPosition.y = teren.SampleHeight(spawnPosition);

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

    private void NotEnoughStamina()
    {

    }
}
