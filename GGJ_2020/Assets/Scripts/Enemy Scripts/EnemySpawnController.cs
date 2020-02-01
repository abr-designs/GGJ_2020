using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{

    public List<GameObject> spawnPointList;
    public Transform enemyContainer;

    // Start is called before the first frame update
    void Start()
    {
        // initialize spawnPointList
        foreach(Transform child in transform) {
            spawnPointList.Add(child.gameObject);
            // set spawn controller
            child.GetComponent<EnemySpawnPoint>().setSpawnController(this.gameObject);
        }

    }

}
