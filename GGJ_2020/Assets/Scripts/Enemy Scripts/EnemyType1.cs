using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : EnemyBaseState
{
    
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
    }

    public override void performAttack() {
        
        // Debug.Log($"Attack from {gameObject.name}");

    }

}
