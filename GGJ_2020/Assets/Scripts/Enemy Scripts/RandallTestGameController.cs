﻿using System.Collections;
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
        // set spawn controller 1 as active
        // changeActiveSpawnerSet(spawnController1);
        Debug.Log("Press key 1 or 2 to activate stage spawners. Press 0 to deactivate stage.");

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
                Debug.Log("Activate stage " + selectedSpawner);
            } else {
                // disable stage spawners
                Debug.Log("Disable stage spawners");
            }


        }

    }

}
