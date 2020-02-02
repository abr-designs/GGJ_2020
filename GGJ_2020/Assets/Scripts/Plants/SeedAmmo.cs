using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeedAmmo : BaseItem
{
    [SerializeField]
    private string targetTag;

    public Transform currentTreeContainer;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        GameObject treeContainer = GameObject.Find("Current Stage Trees");

        Instantiate(prefab, collision.contacts[0].point, Quaternion.identity, treeContainer.transform);
        
        Destroy(gameObject);

        //TODO Plant prefab at location
        //TODO Destroy this object
    }
    
}
