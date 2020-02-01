using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public List<GameObject> spawnPointList;
    public Transform enemyContainer;

    [SerializeField]
    bool isActiveStageSpawner;

    // Start is called before the first frame update
    void Awake()
    {
        // initialize spawnPointList
        foreach(Transform child in transform) {
            spawnPointList.Add(child.gameObject);
            // set spawn controller
            child.GetComponent<EnemySpawnPoint>().setSpawnController(this.gameObject);
        }

    }

    // void Update() {
    //     if(Input.GetKeyDown(KeyCode.A)) { activateSpawners(); }
    // }

    public void setSpawnersActive(bool b) {

        isActiveStageSpawner = b;
        foreach(GameObject spawner in spawnPointList) {
            spawner.GetComponent<EnemySpawnPoint>().setIsSpawning(b);
        }
    }

}
