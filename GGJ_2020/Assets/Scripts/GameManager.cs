using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<EnemyBaseState> enemies { get; private set; }
    public List<PlantBase> plants{ get; private set; }
    public List<EnemySpawnController> spawners{ get; private set; }

    private GameObject playerGameObject;
    private PlayerInventory playerInventory;

    //================================================================================================================//
    // containers
    [FoldoutGroup("Containers References")]
    public Transform enemyProjectilesContainer;
    [FoldoutGroup("Containers References")]
    public Transform robotsContainer;
    [FoldoutGroup("Containers References")]
    public Transform currentStageTreesContainer;
    [FoldoutGroup("Containers References")]
    public Transform archivedTreesContainer;
    [FoldoutGroup("Containers References")]
    public Transform seedAmmoContainer;
    [FoldoutGroup("Containers References")]
    public Transform pickupSeedsContainer;
    //================================================================================================================//


    [FoldoutGroup("Spawn Controllers")]
    public GameObject spawnController1;
    [FoldoutGroup("Spawn Controllers")]
    public GameObject spawnController2;
    
    // [FoldoutGroup("Debug Enemy Spawner")]

    // stage variables
    public int currentStageIndex;
    public GameObject currentStageReference;
    public List<GameObject> stagesRefernceList;
    [SerializeField]
    private GameObject selectedSpawner;

    public Vector3 stageBaseSeedPosition;
    public GameObject currentHomeBaseSeed;
    public GameObject stageHomeBaseTree;

    public int startingStage;

    //================================================================================================================//

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerGameObject = playerInventory.gameObject;

        initStage(startingStage);
    }

    //================================================================================================================//
    
    #region Registration of Objects
    
    public void RegisterEnemy(EnemyBaseState enemy)
    {
        if(enemies== null)
            enemies = new List<EnemyBaseState>();
        
        enemies.Add(enemy);
    }
    public void UnRegisterEnemy(EnemyBaseState enemy)
    {
        enemies?.Remove(enemy);
    }
    
    public void RegisterPlant(PlantBase plant)
    {
        if(plants== null)
            plants = new List<PlantBase>();
        
        plants.Add(plant);
    }
    public void UnRegisterPlant(PlantBase plant)
    {
        plants?.Remove(plant);
    }
    
    public void RegisterPlant(EnemySpawnController enemySpawnController)
    {
        if(spawners== null)
            spawners = new List<EnemySpawnController>();
        
        spawners.Add(enemySpawnController);
    }
    public void UnRegisterPlant(EnemySpawnController enemySpawnController)
    {
        spawners?.Remove(enemySpawnController);
    }
    #endregion //Registration of Objects
    

    //================================================================================================================//
    // stage management

    // function to call when base tree is planted
    public void initStage(int i) {

        Debug.Log($"Set stage as active: {i}");

        currentStageIndex = i;
        currentStageReference = stagesRefernceList[i-1];

        string stageObjectName = "Stage " + currentStageIndex;
        
        // init stage needs to identify the base seed - and subsequent base tree
        GameObject stageObject = GameObject.Find(stageObjectName);
        // GameObject baseSeedContainer = stageObject.chil
        Transform[] children = stageObject.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
            if (child.name == "Base Seed Container")
                stageBaseSeedPosition = child.position;

    }
    
    public void setStageBaseTree(GameObject g) { stageHomeBaseTree = g; }


    public void activateStageSpawners() {

        GameObject spawnsToActivate = null;

        switch(currentStageIndex) {
            case 1:
                spawnsToActivate = spawnController1;
                break;
            case 2:
                spawnsToActivate = spawnController2;
                break;
            default:
                spawnsToActivate = null;
                break;
        }

        changeActiveSpawnerSet(spawnsToActivate);
    }

    void changeActiveSpawnerSet(GameObject g) {

        if(g != selectedSpawner) {
            // deactivate previous spawner
            if(selectedSpawner != null) {
                selectedSpawner.GetComponent<EnemySpawnController>().setSpawnersActive(false);
            }
            
            selectedSpawner = g;
            // activate selected spawn controller
            if(selectedSpawner != null) {
                selectedSpawner.GetComponent<EnemySpawnController>().setSpawnersActive(true);
                // Debug.Log("Activate stage " + selectedSpawner);
            } else {
                // disable stage spawners
                Debug.Log("Disable stage spawners");
            }


        }

    }

    public void deactivateStageSpawners() {
        selectedSpawner.GetComponent<EnemySpawnController>().setSpawnersActive(false);
        selectedSpawner = null;
    }
    
    public void failCurrentStage() {

        Debug.Log($"Failed stage: {currentStageIndex}");

        // deactivate existing spawners
        deactivateStageSpawners();

        // replace base seed of this stage
        initStage(currentStageIndex);

    }

    public void completeCurrentStage() {
        
        Debug.Log($"Complete stage: {currentStageIndex}");

        // deactivate existing spawners
        deactivateStageSpawners();

        // need to activate the base seed of the next stage
        initStage(currentStageIndex + 1);
    }

    //================================================================================================================//
    
    #region Spawn Debug Enemies

    // [FoldoutGroup("Debug Enemy Spawner")]
    // GameObject selectedSpawner;

    [FoldoutGroup("Debug Enemy Spawner")]
    public GameObject debugSpawnPoint;
    [FoldoutGroup("Debug Enemy Spawner")]
    public GameObject prefabDebugEnemyType;

    [FoldoutGroup("Debug Enemy Spawner"), Button("Spawn Debug Enemy")]
    public void spawnDebugEnemy() {

        // check for null value
        if(debugSpawnPoint == null) {
            Debug.LogError("Cannot spawn enemy. No spawn point selected.");
            return;
        }

        debugSpawnPoint.GetComponent<EnemySpawnPoint>().spawnDebugEnemy(prefabDebugEnemyType);
        
    }

    #endregion //Spawn Debug Enemies

    //================================================================================================================//
}
