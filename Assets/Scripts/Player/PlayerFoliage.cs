using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFoliage : MonoBehaviour {

    //Foliage density:
    //  max plants / 1*1 square = foliageDensity * 100
    //  probabilty of plant / 0.2s = foliageDensity
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
    [HideInInspector] public Terrain teren;
    [HideInInspector] private Texture2D foliageMap;
    private RaycastHit foliageSpawnRay;
    private Vector3 spawnPosition;
    private List<Vector3> spawnPositions;
    private float envTemperature;

    // Use this for initialization
    void Start () {
        //Set up Foliage control
        ////////////////////////
        teren = Terrain.activeTerrain;

        foliageMap = new Texture2D((int)Mathf.Floor(teren.terrainData.size.x),
                                   (int)Mathf.Floor(teren.terrainData.size.z));

        Color blackCol = new Color(0, 0, 0);
        Color[] texArray = foliageMap.GetPixels();
        for (var i = 0; i < texArray.Length; i++)
        {
            texArray[i] = blackCol;
        }
        foliageMap.SetPixels(texArray);

        currentFoliageSpawnRadius = foliageSpawnRadius;
        currentFoliageDensity = foliageDensity;
        plants = new Transform[] { plant1, plant2, plant3, plant4, plant5, plant6 };
        spawnPositions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void PlantFoliage(int forceBlop)
    {
        spawnPositions.Clear();
        //Force blop is used to quickly set up massive 
        //Only plant new foliage if foliageMap at x, y permits it
        float foliageValue = foliageMap.GetPixel((int)transform.position.x, (int)transform.position.z).r;
        if (foliageValue < currentFoliageDensity)
        {
            if (forceBlop == 0)
            {
                //Some random foliage generation
                //A pow4 is used as it gives a nice curve turning foliage density to
                //per-frame-spawn-propability at 0->0, 0.5->~0.05, 1.0->1.0
                if (Random.value * Time.fixedDeltaTime < currentFoliageDensity * 0.2f)
                {
                    //Generate random spawn point around player (this can spawn foliage outside of landscape at y zero)
                    //SpawnRadius 1 gives 1 square, 2 gives 3*3, 3 gives 5*5...
                    spawnPosition = new Vector3((int)transform.position.x - (currentFoliageSpawnRadius - 1) + Random.value * (2 * currentFoliageSpawnRadius - 1),
                                                0.0f,
                                                (int)transform.position.z - (currentFoliageSpawnRadius - 1) + Random.value * (2 * currentFoliageSpawnRadius - 1));
                    spawnPosition.y = teren.SampleHeight(spawnPosition);

                    //Increase red value in the foliage map
                    foliageMap.SetPixel((int)spawnPosition.x, (int)spawnPosition.z,
                                        new Color(Mathf.Min(foliageValue + 0.01f,
                                                            1.0f),
                                                  0.0f,
                                                  0.0f));
                    spawnPositions.Add(spawnPosition);
                }
            } else {
                float densityFill = 1.0f;
                if (forceBlop == 1)
                    densityFill = foliageDensity / 4.0f;
                if (forceBlop == 2)
                    densityFill = foliageDensity / 2.0f;
                if (forceBlop == 3)
                    densityFill = foliageDensity * 3.0f / 4.0f;


                //Cover the whole foliageSpawnRadius quadrant
                for (var zI = 0; zI < (2 * currentFoliageSpawnRadius - 1); zI++)
                {
                    for (var xI = 0; xI < (2 * currentFoliageSpawnRadius - 1); xI++)
                    {
                        //Reread foliage
                        foliageValue = foliageMap.GetPixel((int)transform.position.x, (int)transform.position.z).r;

                        //Find number of needed plants
                        int numberOfPlants = (int)((densityFill * ((currentFoliageSpawnRadius - Mathf.Abs(xI - (currentFoliageSpawnRadius - 1)))
                                                                +  (currentFoliageSpawnRadius - Mathf.Abs(zI - (currentFoliageSpawnRadius - 1))))/(2*currentFoliageSpawnRadius)
                                                    - foliageValue) * 100);

                        if (numberOfPlants < 1)
                            numberOfPlants = 0;

                        //spawn a small blop of plants under player
                        for (var j = 0; j < numberOfPlants; j++)
                        {
                            spawnPosition = new Vector3((int)transform.position.x - (currentFoliageSpawnRadius - 1) + xI + Random.value,
                                                        0.0f,
                                                        (int)transform.position.z - (currentFoliageSpawnRadius - 1) + zI + Random.value);
                            spawnPosition.y = teren.SampleHeight(spawnPosition);
                            spawnPositions.Add(spawnPosition);
                        }
                        //Increase red value in the foliage map
                        foliageMap.SetPixel((int)spawnPosition.x, (int)spawnPosition.z,
                                            new Color(Mathf.Max(densityFill, foliageValue), 0.0f, 0.0f));
                    }
                }
            }

            //Spawn all needed plants
            for (var i = 0; i < spawnPositions.Count; i++)
            {
                //Random plant
                int plantType = Random.Range(0, plants.Length);

                //Random yaw
                Quaternion spawnRotation = Quaternion.Euler(plants[plantType].localEulerAngles.x,
                                                            Random.Range(0, 360),
                                                            plants[plantType].localEulerAngles.z);
                //Plant
                Instantiate(plants[plantType], spawnPositions[i] + plants[plantType].position, spawnRotation);
            }
        }
    }

    public float GetEnvironmentTemperature(Vector3 pos)
    {
        //Get environment temperature as avg snow count in an 11x11 block around player
        Color[] surroundingSnow = GetComponent<PlayerFoliage>().foliageMap.GetPixels(
            Mathf.Min(Mathf.Max((int)pos.x - 5, 0),
                      (int)GetComponent<PlayerFoliage>().teren.terrainData.bounds.max.x - 11),
            Mathf.Min(Mathf.Max((int)pos.z - 5, 0),
                      (int)GetComponent<PlayerFoliage>().teren.terrainData.bounds.max.z - 11),
            11, 11);

        envTemperature = 0.0f;
        for (var i = 0; i < surroundingSnow.Length; i++)
        {
            envTemperature += surroundingSnow[i].r;
        }
        envTemperature /= (surroundingSnow.Length * foliageDensity);
        //Mathematically envTemp goes up to 1.0. But 0.9 is a more realistic number with
        //how it's calculated and how foliageMap is drawn on
        envTemperature = Mathf.Clamp(envTemperature, 0.0f, 0.9f);

        return envTemperature;
    }
}
