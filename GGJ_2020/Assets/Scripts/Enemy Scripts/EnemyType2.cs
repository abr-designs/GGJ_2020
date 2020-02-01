using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyType2 : EnemyBaseState
{
    
    // initialize this enemy with specific variables
    public override void initEnemy() {
        setMovementSpeed(2.5f);
        setRotationSpeed(35.0f);
        setAttackRange(5.0f);
        setAttackCooldown(5.0f);
    }

    public override void performAttack() {
        
        Debug.Log("Attack from enemy 2");

    }

}
