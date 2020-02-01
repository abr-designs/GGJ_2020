﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(PlayerInventory))]
public class SeedCannon : MonoBehaviour
{
    [SerializeField] 
    private BaseItem.itemList selectedAmmo;
    //================================================================================================================//

    [SerializeField, Required] private Transform spawnPointTransform;

    [SerializeField] private float shootForce;

    [SerializeField] private KeyCode shootKey;

    private PlayerInventory playerInventory;

    //================================================================================================================//

    //TODO Reference the selected Seed index from Player Inventory

    // Start is called before the first frame update
    private void Start()
    {
        playerInventory = GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(shootKey))
            ShootSeed();
    }

    //================================================================================================================//

    private void ShootSeed()
    {
        //TODO Try to get prefab
        if (!playerInventory.TryGetSeed(0, out var seedPrefab))
        {
            Debug.Log($"Out of ammo");
            return;
        }

        var direction = (spawnPointTransform.forward.normalized * shootForce) + spawnPointTransform.up.normalized;

        var tempRigidbody = Instantiate(seedPrefab, spawnPointTransform.position + spawnPointTransform.forward.normalized , Quaternion.identity)
            .GetComponent<Rigidbody>();

        tempRigidbody.AddForce(direction * shootForce);
    }

    //================================================================================================================//
}
