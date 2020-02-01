using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeedAmmo : BaseItem
{
    [SerializeField]
    private string targetTag;

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        Instantiate(prefab, collision.contacts[0].point, Quaternion.identity);
        
        Destroy(gameObject);

        //TODO Plant prefab at location
        //TODO Destroy this object
    }
    
}
