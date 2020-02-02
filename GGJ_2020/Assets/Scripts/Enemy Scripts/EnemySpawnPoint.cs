using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour, IDamageable
{
    EnemySpawnController spawnController;
    
    public List<RobotPathway> pathwaysForSpawnPoint;
    private int nextPathwayIndex;
    
    public List<GameObject> enemyTypes;

    // int totalEnemies = 5;
    // int remainingEnemies;

    [SerializeField]
    private int spawnMinInterval, spawnMaxInterval;
    private float spawnCooldown;
    private float spawnCooldownCounter;
    public float fertilityDamageInterval = 5.0f;
    public float fertilityDamageCounter;
    [SerializeField]
    private float spawnSpeedMultiplier; // speed multiplier increase when damaged by fertility

    [SerializeField]
    private bool isSpawning;

    public float factoryIntegrity; // use for factory damage in order to cause factory to be destoryed

    // reference static
    private static GameManager gm;

    private FertilityController fertilityController;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gm == null) {
            gm = FindObjectOfType<GameManager>();
        }
        if(fertilityController == null) {
            fertilityController = FindObjectOfType<FertilityController>();
        }
        
        initStats();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isSpawning) {
            return;
        }

        updateCounters();

        // Debug.Log(spawnCooldownCounter);

        if(spawnCooldownCounter <= 0) { //} && remainingEnemies > 0) {
            spawnEnemy();
        }

        if(fertilityDamageCounter <= 0) { //} && remainingEnemies > 0) {
            dealFertilityDamage();
        }
    }
    
    void initStats() {

        // totalEnemies = 5;
        // remainingEnemies = totalEnemies;
        //spawnCooldown = 
        setSpawnCooldown();
        // Debug.Log(spawnCooldown);



        // isSpawning = true;
        spawnSpeedMultiplier = 1.0f;

        fertilityDamageCounter = fertilityDamageInterval;

        factoryIntegrity = 1.0f; // use for factory damage in order to cause factory to be destoryed

        nextPathwayIndex = 0;

    }

    public void setIsSpawning(bool b) { isSpawning = b; }
    public void setSpawnController(GameObject g) {
        spawnController = g.GetComponent<EnemySpawnController>();
    }
    public void setSpawnSpeedMultiplier(float f) { spawnSpeedMultiplier = f; }
    public float getSpawnSpeedMultiplier() { return spawnSpeedMultiplier; }

    // update counters being tracked for this enemy
    void updateCounters() {
        if(spawnCooldownCounter > 0) { spawnCooldownCounter -= Time.deltaTime * spawnSpeedMultiplier; }

        if(fertilityDamageCounter > 0) { fertilityDamageCounter -= Time.deltaTime; }
    }

    void setSpawnCooldown() {
        spawnCooldown = (float)Random.Range(spawnMinInterval*10, spawnMaxInterval*10) / 10.0f;
        spawnCooldownCounter = spawnCooldown;
    }

    void spawnEnemy() {

        // determine what type of enemy to spawn
        //

        // check to make sure there is an enemy type available to spawn
        if(enemyTypes.Count < 1) {
            Debug.Log("No enemy types available to spawn from " + name);
            isSpawning = false;
        } else {
            GameObject enemyToSpawn = enemyTypes[0];

            // spawn the selected type of enemy
            Vector3 spawnPositionOffset = new Vector3(0,1,0);//1,0);
            Vector3 spawnPosition = transform.position + spawnPositionOffset;

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, gm.robotsContainer);//spawnController.enemyContainer)

            // set pathway for newEnemy
            RobotPathway newPathway = pathwaysForSpawnPoint[nextPathwayIndex];
            newEnemy.GetComponent<EnemyBaseState>().setPathway(newPathway);
            newEnemy.GetComponent<EnemyBaseState>().setSpawnPoint(gameObject);

            // increment lastUsedPathIndex
            nextPathwayIndex += 1;
            if(nextPathwayIndex >= pathwaysForSpawnPoint.Count) { nextPathwayIndex = 0; }

            // identify first waypoint
            GameObject firstWaypoint = newPathway.pathway[0].gameObject;

            // rotate spawned enemy to look at first waypoint
            Vector3 localTarget = newEnemy.transform.InverseTransformPoint(firstWaypoint.transform.position);
            float angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
            newEnemy.transform.Rotate(Vector3.up * angleToTarget);

            // reduce the number of remainingEnemies
            // remainingEnemies -= 1;

            // // check if remainingEnemies < 1
            // if(remainingEnemies < 1) {
            //     // destroy or disable the spawner
            //     isSpawning = false;
            // }

            // reset spawn countdown
            // spawnCooldownCounter = spawnCooldown;
            setSpawnCooldown();
        }
    }

    public void spawnDebugEnemy(GameObject g) {

        GameObject enemyToSpawn = g;

        // check for null value
        if(enemyToSpawn == null) {
            Debug.LogError("Cannot spawn enemy. No enemy type selected.");
            return;
        }

        // spawn the selected type of enemy
        Vector3 spawnPositionOffset = new Vector3(0,1,0);
        Vector3 spawnPosition = transform.position + spawnPositionOffset;

        GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, gm.robotsContainer);//spawnController.enemyContainer);

            // set pathway for newEnemy
            RobotPathway newPathway = pathwaysForSpawnPoint[nextPathwayIndex];
            newEnemy.GetComponent<EnemyBaseState>().setPathway(newPathway);
            newEnemy.GetComponent<EnemyBaseState>().setSpawnPoint(gameObject);

            // increment lastUsedPathIndex
            nextPathwayIndex += 1;
            if(nextPathwayIndex >= pathwaysForSpawnPoint.Count) { nextPathwayIndex = 0; }

            // identify first waypoint
            GameObject firstWaypoint = newPathway.pathway[0].gameObject;

        // rotate spawned enemy to look at first waypoint
        Vector3 localTarget = newEnemy.transform.InverseTransformPoint(firstWaypoint.transform.position);
        float angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        newEnemy.transform.Rotate(Vector3.up * angleToTarget);
    }

    void dealFertilityDamage() {

        // fertilityController.GetFloatAtPos(transform.position);
        float fertilityDamageAmount = fertilityController.GetFloatAtPos(transform.position);;

        // check fertility in range of factory to determine how much damage to deal
        Debug.Log($"Deal [{fertilityDamageAmount}] fertility damage to factory");

        Damage(fertilityDamageAmount);

        // reset countdown
        fertilityDamageCounter = fertilityDamageInterval;

    }
    public void Damage(float amount)
    {
        factoryIntegrity -= amount;

        // Debug.Log($"Deal [{amount}] damage to [{name}]. Remaining health = [{health}]");

        if(factoryIntegrity <= 0) {
            // factory is destroyed
            crumbleFactory();
        }
    }
    public void Heal(float amount)
    {
        factoryIntegrity += amount;
    }

    public void crumbleFactory() {

        // factory is destroyed
        Debug.Log($"Crumble {this}");
        // call animation to move the factory beneath the ground
        
        // placeholder to just make mesh not visible - destroy spawn point children
        foreach (Transform child in transform) {
            GameObject.Destroy(child.gameObject);
        }

        // set spawning to false
        isSpawning = false;
        
        // call stage manager to check for all spawn points having been destroyed
        gm.checkStageCompletion();

    }

    // debug damage factory
    [FoldoutGroup("Debug Reduce Factory Integrity"), Button("Factory Reduce Integrity by 0.25/1.0")]
    public void debugDamageFactory() {
        Damage(0.25f);
    }

    [FoldoutGroup("Debug Destroy Factory"), Button("Factory Deathblow")]
    public void debugFactoryDeathblow() {
        Damage(factoryIntegrity);
    }

}
