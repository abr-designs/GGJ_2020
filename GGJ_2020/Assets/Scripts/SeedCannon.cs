using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class SeedCannon : MonoBehaviour
{
    //TODO Reference the selected Seed index from Player Inventory

    [SerializeField] 
    private BaseItem.itemList selectedAmmo;
    //================================================================================================================//

    [SerializeField, Required] private Transform spawnPointTransform;

    [SerializeField] private float shootForce;

    [SerializeField] private KeyCode shootKey;

    private PlayerInventory playerInventory;

    // reference static
    private static GameManager gm;

    //================================================================================================================//


    // Start is called before the first frame update
    private void Start()
    {
        if(gm == null) {
            gm = FindObjectOfType<GameManager>();
        }

        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(shootKey))
            ShootSeed();

        if (!Input.GetKeyDown(KeyCode.Tab)) 
            return;
        
        var val = (int)selectedAmmo;
        val++;
        if (val > 3)
            val = 0;

        selectedAmmo = (BaseItem.itemList)val;
    }

    //================================================================================================================//

    private void ShootSeed()
    {
        //TODO Try to get prefab
        if (!playerInventory.TryGetSeed(selectedAmmo, out var seedPrefab))
        {
            Debug.Log($"Out of ammo");
            return;
        }

        var direction = (spawnPointTransform.forward.normalized * shootForce) + spawnPointTransform.up.normalized;

        var tempRigidbody = Instantiate(seedPrefab, spawnPointTransform.position + spawnPointTransform.forward.normalized , Quaternion.identity, gm.seedAmmoContainer)
            .GetComponent<Rigidbody>();

        tempRigidbody.AddForce(direction * shootForce);
        tempRigidbody.AddTorque(new Vector3(Random.Range(-90f,90f), Random.Range(-90f,90f), Random.Range(-90f,90f)));
    }

    //================================================================================================================//
}
