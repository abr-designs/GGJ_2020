using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    EnemySpawnController spawnController;
    
    public RobotPathway pathwayForSpawnPoint;
    
    public List<GameObject> enemyTypes;

    int totalEnemies = 5;
    int remainingEnemies;

    private float spawnCooldown;
    private float spawnCooldownCounter;

    [SerializeField]
    private bool isSpawning;

    // Start is called before the first frame update
    void Start()
    {
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

        if(spawnCooldownCounter <= 0 && remainingEnemies > 0) {
            spawnEnemy();
        }
    }
    
    void initStats() {

        // totalEnemies = 5;
        remainingEnemies = totalEnemies;

        spawnCooldown = Random.Range(20,50) / 10;
        // Debug.Log(spawnCooldown);
        spawnCooldownCounter = spawnCooldown;

        // isSpawning = true;
    }

    public void setIsSpawning(bool b) { isSpawning = b; /*Debug.Log($"Set spawning {isSpawning}");*/ }
    public void setSpawnController(GameObject g) {
        spawnController = g.GetComponent<EnemySpawnController>();
    }

    // update counters being tracked for this enemy
    void updateCounters() {
        if(spawnCooldownCounter > 0) { spawnCooldownCounter -= Time.deltaTime; }
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
            Vector3 spawnPositionOffset = new Vector3(0,1,0);
            Vector3 spawnPosition = transform.position + spawnPositionOffset;

            GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, spawnController.enemyContainer);

            // set pathway for newEnemy
            newEnemy.GetComponent<EnemyBaseState>().setPathway(pathwayForSpawnPoint);

            // reduce the number of remainingEnemies
            remainingEnemies -= 1;

            // check if remainingEnemies < 1
            if(remainingEnemies < 1) {
                // destroy or disable the spawner
                isSpawning = false;
            }

            // reset spawn countdown
            spawnCooldownCounter = spawnCooldown;
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

        GameObject newEnemy = Instantiate(enemyToSpawn, spawnPosition, Quaternion.identity, spawnController.enemyContainer);
    }
}
