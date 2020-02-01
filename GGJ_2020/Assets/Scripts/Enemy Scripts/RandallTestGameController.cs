using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RandallTestGameController : MonoBehaviour
{

    public GameObject spawnController1;
    public GameObject spawnController2;


    [SerializeField]
    GameObject selectedSpawner;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha0)) { changeActiveSpawnerSet(null); }
        else if(Input.GetKeyDown(KeyCode.Alpha1)) { changeActiveSpawnerSet(spawnController1); }
        else if(Input.GetKeyDown(KeyCode.Alpha2)) { changeActiveSpawnerSet(spawnController2); }

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
            }

        }
    }

}
