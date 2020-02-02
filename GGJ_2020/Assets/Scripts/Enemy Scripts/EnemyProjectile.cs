using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Recycling;

public class EnemyProjectile : MonoBehaviour
{

    private float projectileDamage;// = 25.0f;

    public void setProjectileDamage(float f) { projectileDamage = f; }
    
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
