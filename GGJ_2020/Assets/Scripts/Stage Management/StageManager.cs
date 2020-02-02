using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{

    // variable existing in heirarchy
    public GameObject baseSeedContainerReference;
    public GameObject enemySpawnControllerReference;
    public GameObject pathwaysContainerReference;

    // variables to be set during runtime
    public GameObject currentBaseTree;

    // reference static
    private static GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gm == null) {
            gm = FindObjectOfType<GameManager>();
        }
    }

    public void checkCompletion() {
        // check the integrety of all spawn points
        bool anySpawnPointAlive = false;

        // Debug.Log($"Length of spawn point list = {enemySpawnControllerReference.GetComponent<EnemySpawnController>().spawnPointList.Count}");

        foreach(GameObject spawnPointObject in enemySpawnControllerReference.GetComponent<EnemySpawnController>().spawnPointList) {

            // Debug.Log($"{spawnPointObject} = {spawnPointObject.GetComponent<EnemySpawnPoint>().factoryIntegrity}");
            if(spawnPointObject.GetComponent<EnemySpawnPoint>().factoryIntegrity > 0) {
                anySpawnPointAlive = true;
                break;
            }
        }

        // no spawn points left alive
        if(!anySpawnPointAlive) {
            // complete level
            gm.completeCurrentStage();
        }
    }

}
