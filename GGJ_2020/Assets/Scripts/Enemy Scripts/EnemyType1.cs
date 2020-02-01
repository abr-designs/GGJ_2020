using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : EnemyBaseState
{
    
    [SerializeField]
    private GameObject projectilePrefab;

    private GameObject projectileContainer;

    private float projectileUpwardForce = 150.0f;
    private float projectileForwardForceMultiplier = 25.0f;

    // initialize this enemy with specific variables
    public override void initEnemy() {
        // set movement stats
        setMovementSpeed(5.0f);
        setRotationSpeed(50.0f);
        // set health stats
        setHealth(10.0f);
        setDefense(10.0f);
        // set attack stats
        setAttackRange(2.5f);
        setAttackCooldown(2.0f);

        // set referneces to other objects
        projectileContainer = GameObject.Find("Projectile Container");
    }

    public override void performAttack() {
        
        // Debug.Log($"Attack from {gameObject.name}");

        if(projectilePrefab == null) {
            Debug.LogError("Projectile prefab is null");
            return;
        }

        // this enemy type launches projectiles

        // set projection creation position
        Vector3 spawnPositionOffset = new Vector3(0,2,0);
        Vector3 spawnPosition = transform.position + spawnPositionOffset;
        
        GameObject newProjectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation, projectileContainer.transform); // Quaternion.identity);

        // identify projectile rigidbody
        Rigidbody rb = newProjectile.GetComponent<Rigidbody>();

        // check distance to target
        float distance = Vector3.Distance(newProjectile.transform.position, target.transform.position);
        float forwardForce = distance * projectileForwardForceMultiplier;
        
        // set target and trajectory and projectile
        rb.AddForce(transform.up * projectileUpwardForce);
        rb.AddForce(transform.forward * forwardForce);

    }

}
