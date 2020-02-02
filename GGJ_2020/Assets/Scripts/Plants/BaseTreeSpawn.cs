using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class BaseTreeSpawn : MonoBehaviour
{
    
    public GameObject seedMesh;
    float rotationSpeed = 90.0f;

    public GameObject baseTreeSeedPrefab;
    public List<GameObject> toolkitSeeds;
    public Vector3 toolkitSeedLaunchForce;

  
    public int stageToBegin;

    // public RandallTestGameController randallTestGameController;
    public Transform currentTreeContainer;
    public Transform seedAmmoContainer;

    public GameObject willowSeedPrefab;

    // reference static
    private static GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gm == null) {
            gm = FindObjectOfType<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        rotateSeed();
    }

    void rotateSeed() {
        if(seedMesh != null)
            seedMesh.transform.Rotate(new Vector3(1,2,3) * Time.deltaTime * rotationSpeed);
    }

    private void OnTriggerEnter(Collider other) {

        // check if player has entered trigger
        if(other.gameObject.tag == "Player") {
            plantBaseSeed();
        }


    }

    void plantBaseSeed() {
        
        gm.currentHomeBaseSeed = gameObject;

        // remove existing rotating mesh 
        Destroy(seedMesh);

        // instantiate 
        GameObject newSeed = Instantiate(baseTreeSeedPrefab, seedMesh.transform.position, Quaternion.identity, gm.seedAmmoContainer);
        gm.setStageBaseTree(newSeed);
        
        // delay for 1 second
        StartCoroutine(Wait(0.025f, () => {
            // launch seeds
            launchSeeds();

            // call game manager to initialize enemy spawns
            
            gm.activateStageSpawners();//stageToBegin);
            
            Destroy(gameObject);
        }));

    }

    IEnumerator Wait(float duration, Action OnWaited)
    {
        yield return new WaitForSeconds(duration);
        OnWaited?.Invoke();
    }

    void launchSeeds() {

        Debug.Log($"Launch {toolkitSeeds.Count} toolkit seeds");
        // lauch each toolkit seed
        foreach(GameObject seedAmmoPrefab in toolkitSeeds) {

            GameObject newSeed = null;
            Vector3 newLauchForce;

            // special condition for first willow seed
            // Debug.Log($"Check special condition: {gm.currentStageIndex}, {seedAmmoPrefab}");
            if(gm.currentStageIndex == 0) {//} && seedAmmoPrefab == willowSeedPrefab) {
                
                Debug.Log("Special seed launch condition");
                // launch seeds without special cases
                // randomize spawn position
                // float randX = 50.0f;
                // float randY = 0.0f;
                // float randZ = 50.0f;
                float randX = (float)Random.Range(-20,20)/40.0f;
                float randY = (float)Random.Range(20,40)/40.0f;
                float randZ = (float)Random.Range(-20,20)/40.0f;
                Vector3 randomPosition = new Vector3(randX, randY, randZ);
                Vector3 spawnPosition = transform.position + randomPosition;
                newSeed = Instantiate(seedAmmoPrefab, spawnPosition, Quaternion.identity, gm.seedAmmoContainer);

                // add force to launched seed
                randX = 250;
                randY = 500;
                randZ = 250;
                newLauchForce = new Vector3(randX, randY, randZ);

            } else {
                // launch seeds without special cases
                // randomize spawn position
                float randX = (float)Random.Range(-20,20)/40.0f;
                float randY = (float)Random.Range(20,40)/40.0f;
                float randZ = (float)Random.Range(-20,20)/40.0f;
                Vector3 randomPosition = new Vector3(randX, randY, randZ);
                Vector3 spawnPosition = transform.position + randomPosition;
                newSeed = Instantiate(seedAmmoPrefab, spawnPosition, Quaternion.identity, gm.seedAmmoContainer);

                // add force to launched seed
                randX = (float)Random.Range(-250,-750)/2;
                randY = (float)Random.Range(0,250)/10;
                randZ = (float)Random.Range(-250,-750)/2;
                newLauchForce = new Vector3(toolkitSeedLaunchForce.x + randX,
                    toolkitSeedLaunchForce.y + randY,
                    toolkitSeedLaunchForce.z + randZ);
            }

            Debug.Log($"Launch force = {newLauchForce}");
            newSeed.GetComponent<Rigidbody>().AddForce(newLauchForce);
            newSeed.GetComponent<Rigidbody>().AddTorque(newLauchForce);

        }

    }
}
