using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType1 : EnemyBaseState
{
    
    // initialize this enemy with specific variables
    public override void initEnemy() {
        setMovementSpeed(5.0f);
        setRotationSpeed(50.0f);
        setAttackRange(2.5f);
        setAttackCooldown(2.0f);
    }

    public override void performAttack() {
        
        Debug.Log("Attack from enemy 1");

    }

}
