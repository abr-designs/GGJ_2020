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
    public Transform stagesContainer;
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
    public List<GameObject> stagePrefabs;
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

    private void Update() {
        // check if stage base tree 
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
    public void initStage(int stageNum) {

        // remove existing current stage
        if(currentStageReference != null ) {
            Destroy(currentStageReference);
        }

        Debug.Log($"Set stage as active: {stageNum}");

        currentStageIndex = stageNum;
        
        // check if stagesRefernceList is emtpty
        if(stagePrefabs.Count == 0) return;

        currentStageReference = Instantiate(stagePrefabs[currentStageIndex], Vector3.zero, Quaternion.identity, stagesContainer);

        setStageBaseTree(currentStageReference.GetComponent<StageManager>().baseSeedContainerReference);

        // string stageObjectName = "Stage " + currentStageIndex;
        
        // init stage needs to identify the base seed - and subsequent base tree
        // GameObject stageObject = GameObject.Find(stageObjectName);
        // GameObject baseSeedContainer = stageObject.chil
        // Transform[] children = stageObject.GetComponentsInChildren<Transform>();
        // foreach (Transform child in children)
        //     if (child.name == "Base Seed Container")
        //         stageBaseSeedPosition = child.position;



    }
    
    public void setStageBaseTree(GameObject g) { 
        // Debug.Log($"Set home base tree as {g}");
        stageHomeBaseTree = g;

        // set this on the stage manager
        currentStageReference.GetComponent<StageManager>().setHomeBaseTree(g);
    }


    public void activateStageSpawners() {

        GameObject spawnsToActivate = null;

        // switch(currentStageIndex) {
        //     case 1:
        //         spawnsToActivate = spawnController1;
        //         break;
        //     case 2:
        //         spawnsToActivate = spawnController2;
        //         break;
        //     default:
        //         spawnsToActivate = null;
        //         break;
        // }

        spawnsToActivate = currentStageReference.GetComponent<StageManager>().enemySpawnControllerReference;//.GetComponent>EnemySpawnController>()

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

    public void checkStageCompletion() {

        currentStageReference.GetComponent<StageManager>().checkCompletion();

    }

    public void checkStageFailure() {

        currentStageReference.GetComponent<StageManager>().checkFailure();

    }
    
    public void failCurrentStage() {

        Debug.Log($"Failed stage: {currentStageIndex}");

        // deactivate existing spawners
        deactivateStageSpawners();

        // destroy all seeds in player inventory
        // player.witherInventory()
        playerInventory.ClearInventory();

        // remove all seed pickups in the world
        destroyRemainingSeedPickups();

        // destory all current robots
        foreach(Transform robot in robotsContainer) {
            Destroy(robot.gameObject);
        }

        // destroy all current stage trees
        // foreach(GameObject tree in currentStageTreesContainer) {
            // destroy - or deal interative damage until dead
        // }
        // move all current stage trees into archived trees and set as no longer fruiting
        foreach(Transform tree in currentStageTreesContainer) {
            Destroy(tree.gameObject);
        }

        // remove all seed pickups in the world
        destroyRemainingSeedPickups();

        // wait until the stage has reverted to initial state
        // reverting a stage will need to include restoring destroed factories

        // replace base seed of this stage
        initStage(currentStageIndex);

    }

    public void completeCurrentStage() {
        
        Debug.Log($"Complete stage: {currentStageIndex}");

        // deactivate existing spawners
        // deactivateStageSpawners();

        // destory all current robots
        foreach(Transform robot in robotsContainer) {
            Destroy(robot.gameObject);
        }

        // eject all remaining seeds in player inventory
        playerInventory.ejectPlayerSeeds();
        //playerInventory.ClearInventory();

        // move all current stage trees into archived trees and set as no longer fruiting
        foreach(Transform tree in currentStageTreesContainer) {
            // set as no longer fruiting
            // tree.GetComponent<PlantBase>().IsFruiting = false;
            // move to archivedTreesContainer
            tree.parent = archivedTreesContainer;
        }

        // remove all seed pickups in the world
        destroyRemainingSeedPickups();

        // need to activate the base seed of the next stage
        initStage(currentStageIndex + 1);
    }

    private void destroyRemainingSeedPickups() {
        foreach (Transform child in pickupSeedsContainer) {
            GameObject.Destroy(child.gameObject);
        }
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
