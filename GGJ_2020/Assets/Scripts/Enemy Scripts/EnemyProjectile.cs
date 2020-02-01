using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{

    float projectileDamage = 1.0f;

    private void OnCollisionEnter(Collision other) {

        Debug.Log($"Collide with {other.gameObject}");
        
        // if collides with planet, then deal damage
        if(other.gameObject.GetComponent<PlantBase>()) {
            other.gameObject.GetComponent<PlantBase>().Damage(projectileDamage);
        }

        // destroy projectile after collision
        Destroy(gameObject);


    }
}
