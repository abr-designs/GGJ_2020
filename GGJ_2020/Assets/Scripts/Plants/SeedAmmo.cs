using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeedAmmo : BaseItem
{
    [SerializeField]
    private string targetTag;

    public Transform currentTreeContainer;

    // reference static
    private static GameManager gm;
    
    // Start is called before the first frame update
    void Start()
    {
        if(gm == null) {
            gm = FindObjectOfType<GameManager>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        GameObject treeContainer = GameObject.Find("Current Stage Trees");

        Instantiate(prefab, collision.contacts[0].point, Quaternion.identity, gm.currentStageTreesContainer);
        
        Destroy(gameObject);

        //TODO Plant prefab at location
        //TODO Destroy this object
    }
    
}
