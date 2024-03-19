using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetHandler : MonoBehaviour
{
    [SerializeField] Transform buildingParentLayer;
    public bool isOnboardingMode;

    [SerializeField] List<PlanetLayer> planetLayers = new List<PlanetLayer>();
    [SerializeField] List<int> layerRotationSpeed = new List<int>();
  
    [SerializeField] Vector3 gamePosition;
    [SerializeField] Vector3 menuPosition;
    bool newPosition;
    Vector3 tempNewPos;


    [Header("Building Generation")]
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject[] structures;
    [Range(0, 1)]
    [SerializeField] float percentageOfBuildingSuccess;
    [SerializeField] float minSpawnDelay;
    [SerializeField] float maxSpawnDelay;
    
    [SerializeField] float[] structureScales;
    [SerializeField] float[] narrowStructureScales;

    float timeElapsedFromLastStructure;
    bool spawnBuildings;
    bool narrowBuildingSpawn;
    float nextStructureSpawnTime;
    bool isAreaClear = false;
    [SerializeField] float minYOffset;
    [SerializeField] float maxYOffset;

    [SerializeField] List<BuildingObject> activeBuildings = new List<BuildingObject>();

    [Header("Plans Generation")]
    [SerializeField] Sprite[] plantSprites;
    [SerializeField] Transform plantSpawn;
    [SerializeField] GameObject plantPrefab;

    [Range(0,1)]
    [SerializeField] float percentageOfPlantSuccess;
    [SerializeField] float minSpawnPlantDelay;
    [SerializeField] float maxSpawnPlantDelay;

    [SerializeField] float[] plantScales;
    [SerializeField] Color[] plantColors;

    [SerializeField] float plantMinXOffset;
    [SerializeField] float plantMaxXOffest;
    float timeElapsedFromLastPlant;
    bool spawnPlants;
    float nextPlantSpawnTime;

    int internalBuildingSortingOrder = 2; //2 to 4

    public GameObject lastBuildingSpawned
    {
        get
        {
            return activeBuildings[activeBuildings.Count - 1].gameObject;
        }
    }

    private void Start()
    {
        internalBuildingSortingOrder = 2;
        transform.position = menuPosition;
        spawnPlants = false;
        GameManager.OnGameStateChanged += GameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameStateChanged;
    }

    public void GameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.GAME:
                StartGame();
                break;
        }
    }
    public void StartGame()
    {
        StartPlanetMovement();
        internalBuildingSortingOrder = 2;
        isAreaClear = true;
        newPosition = true;
        tempNewPos = gamePosition;
        spawnPlants = true;
        spawnBuildings = true;
    }

    public void OnboardingStart()
    {
        StartPlanetMovement();

        isAreaClear = true;
        newPosition = true;
        tempNewPos = gamePosition;
        spawnPlants = true;
        spawnBuildings = true;

        isOnboardingMode = true;
    }

    public void GameFinale(bool destroyAll)
    {
        spawnPlants = false;
        spawnBuildings = false;

        if (destroyAll)
        {
            foreach (Transform obj in buildingParentLayer)
            {
                BuildingObject building = obj.GetComponent<BuildingObject>();
                if (building == null)
                {
                    Destroy(obj.gameObject);
                }
                else
                {
                    building.DestroyNoScore();
                }

            }
        }
       
    }

    public void OnboardingExit()
    {
        GameFinale(false);
        tempNewPos = menuPosition;
        newPosition = true;
      
        foreach (Transform obj in buildingParentLayer)
        {
            Destroy(obj.gameObject);
        }

        isOnboardingMode = false;
    }

    private void Update()
    {
        if (newPosition)
        {
            transform.position = Vector3.Lerp(transform.position, tempNewPos, 10f);
            if (transform.position.y >= gamePosition.y)
            {
                newPosition = false;
            }
        }

        if (spawnPlants)
        {
            if (timeElapsedFromLastPlant < nextPlantSpawnTime)
            {
                timeElapsedFromLastPlant += Time.deltaTime;
            }
            else
            {
                if (Random.Range(0f, 1f) < percentageOfPlantSuccess)
                {
                    
                    SpawnPlant();
                }

                timeElapsedFromLastPlant = 0;
                nextPlantSpawnTime = Random.Range(minSpawnPlantDelay, maxSpawnPlantDelay);
            }
        }

        if (spawnBuildings)
        {
            if (timeElapsedFromLastStructure < nextStructureSpawnTime)
            {
                timeElapsedFromLastStructure += Time.deltaTime;
            }
            else
            {
                if (Random.Range(0f, 1f) < percentageOfBuildingSuccess)
                {
                   

                    if (isAreaClear)
                    {
                        isAreaClear = false;
                        SpawnBuilding();
                    }
                   
                }

                timeElapsedFromLastStructure = 0;
                nextStructureSpawnTime = Random.Range(minSpawnDelay, maxSpawnDelay);
            }
        }
    }

    public void TryRemoveBuilding(BuildingObject building)
    {
        if (activeBuildings.Contains(building))
        {
            activeBuildings.Remove(building);
        }
    }

    public void StartPlanetMovement()
    {
        if (planetLayers == null || planetLayers.Count == 0)
        {
            Debug.LogError("No Planet Layers Exist. Assign them in the Inspector");
            return;
        }

        if (layerRotationSpeed.Count != planetLayers.Count)
        {
            Debug.LogError("The rotation speed for all layers has not been set. Assign speed in the inspector");
            return;
        }

        for (int i = 0; i < planetLayers.Count; i++)
        {
            planetLayers[i].SetSpeed(layerRotationSpeed[i]);
        }
    }

    public void StopPlanetMovement()
    {
        for (int i = 0; i < planetLayers.Count; i++)
        {
            planetLayers[i].SetSpeed(0);
        }
    }


    public void StartBuildingSpawn()
    {
        StartCoroutine(SpawnEnum());
    }

    IEnumerator SpawnEnum() 
    {
        yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        SpawnBuilding();
    }


    public void AreaClear()
    {
        isAreaClear = true;
    }

    void SpawnPlant()
    {
        GameObject obj = Instantiate(plantPrefab, (plantSpawn.transform.position + new Vector3(Random.Range(plantMinXOffset,plantMaxXOffest), 0, 0)), Quaternion.identity);
        obj.GetComponent<SpriteRenderer>().sprite = plantSprites[Random.Range(0, plantSprites.Length)];
        obj.GetComponent<SpriteRenderer>().color = plantColors[Random.Range(0, plantColors.Length)];
        obj.transform.parent = buildingParentLayer;
        obj.transform.Rotate(0, 0, Random.Range(-105, -90));
        int randomScaleChosen = Random.Range(0, plantScales.Length);
        obj.transform.localScale = new Vector3(plantScales[randomScaleChosen], plantScales[randomScaleChosen], 1);
    }

    void SpawnBuilding()
    {
        int randomStructure = Random.Range(0, structures.Length);
        GameObject obj;
        int randomScaleChosen = 1;
        float randomScale = 1;
        if (!narrowBuildingSpawn)
        {
            randomScaleChosen = Random.Range(0, structureScales.Length);
            randomScale = structureScales[randomScaleChosen];
            obj = Instantiate(structures[0], (spawnPosition.transform.position + new Vector3(Random.Range(minYOffset, maxYOffset), 0,-1)), Quaternion.identity);
            narrowBuildingSpawn = true;
        }
        else
        {
            randomScaleChosen = Random.Range(0, narrowStructureScales.Length);
            randomScale = narrowStructureScales[randomScaleChosen];
            obj = Instantiate(structures[1], (spawnPosition.transform.position + new Vector3(Random.Range(minYOffset, maxYOffset), 0, -1)), Quaternion.identity);
            narrowBuildingSpawn = false;
        }
     
        obj.transform.parent = buildingParentLayer;
        obj.transform.Rotate(0, 0, -110);
        obj.transform.localScale = new Vector3(randomScale, randomScale, 1);
        activeBuildings.Add(obj.GetComponent<BuildingObject>());
        obj.GetComponent<BuildingObject>().planetHandler = this;
        obj.GetComponent<BuildingObject>().SetSortingOrder(internalBuildingSortingOrder);
       obj.GetComponent<BuildingObject>().RandomizeColor();

        internalBuildingSortingOrder += 1;
        if (internalBuildingSortingOrder > 4)
        {
            internalBuildingSortingOrder = 2;
        }
    }
}
