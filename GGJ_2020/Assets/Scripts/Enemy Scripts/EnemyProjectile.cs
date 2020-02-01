using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recycling;

public class EnemyProjectile : MonoBehaviour
{

    float projectileDamage = 1.0f;
    
    private void OnTriggerEnter(Collider other) {

        // Debug.Log($"{gameObject} collided with {other.name} with tag {other.tag}");

        // check if colliding with something to destroy it
        bool collideWithDestroyer = false;

        // if collides with planet, then deal damage
        if(other.gameObject.tag == "Tree") {
            other.gameObject.GetComponent<PlantBase>().Damage(projectileDamage);
            collideWithDestroyer = true;
        } else if(other.gameObject.tag == "Ground") {
            collideWithDestroyer = true;
        }

        // destroy projectile after collision
        if(collideWithDestroyer)
            Destroy(gameObject);


    }
}
