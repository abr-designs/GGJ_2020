using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SeedAmmo : BaseItem
{
    [SerializeField]
    private bool ignoreFertility;
    [SerializeField]
    private float spawnRequiredRadius;
    
    [SerializeField]
    private string targetTag;

    public Transform currentTreeContainer { get; private set; }

    [SerializeField, Required]
    private GameObject PickupPrefab;

    private new Transform transform;
    private new Rigidbody rigidbody;

    //================================================================================================================//

    // reference static
    private static GameManager gm;
    private static FertilityController FertilityController;
    
    // Start is called before the first frame update
    private void Start()
    {
        if(gm == null) {
            gm = FindObjectOfType<GameManager>();
        }

        if (!FertilityController)
            FertilityController = FindObjectOfType<FertilityController>();
        
        if(!PickupPrefab)
            throw new MissingReferenceException($"Pickup prefab not set on {gameObject.name}");

        transform = gameObject.transform;
        rigidbody = gameObject.GetComponent<Rigidbody>();
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(targetTag))
            return;

        var point = collision.contacts[0].point;

        if (!CheckCanSpawn(point))
        {
            gameObject.GetComponent<Collider>().enabled = false;
            var pickup = Instantiate(PickupPrefab, transform.position, transform.rotation, transform.parent);
            pickup.GetComponent<BoxCollider>().enabled = true;
            var rb = pickup.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.velocity = rigidbody.velocity;
            rb.angularVelocity = rigidbody.angularVelocity;
            
            
            Destroy(gameObject);
            return;
        }

        currentTreeContainer = GameObject.Find("Current Stage Trees")?.transform;
        

        if (!currentTreeContainer)
        {
            Instantiate(prefab, point, Quaternion.identity);
            Debug.LogError("No tree container found");

        }
        else
        {
            Instantiate(prefab, collision.contacts[0].point, Quaternion.identity, gm.currentStageTreesContainer);
            
        }
        
        Destroy(gameObject);

        //TODO Plant prefab at location
        //TODO Destroy this object
    }

    private bool CheckCanSpawn(Vector3 point)
    {
        if (!ignoreFertility && FertilityController.CanPlantAt(point))
            return true;
        
        var plants = gm.plants;

        if (plants == null || plants.Count == 0)
            return true;

        return plants.Any(p => Vector3.Distance(p.transform.position, point) > (p.lockRadius + spawnRequiredRadius));
    }

}
