using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Unity.Mathf;

public abstract class EnemyBaseState : MonoBehaviour
{
    
public enum enemyState {
    idle,
    pursue,
    attack
};

private enemyState state;
public GameObject target;
private float movementSpeed = 5.0f;
private float lookAtThreshold = 2.5f;

private float rotationSpeed = 50.0f;
private float attackRange = 2.5f;
private float attackCooldown;
private float attackCooldownCounter;

    // Start is called before the first frame update
    void Start()
    {
        defaultEnemyStats();
        initEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        updateCounters();

        if(target != null) {
            if(state == enemyState.pursue) {
                pursueTarget();
            } else if(state == enemyState.attack) {
                attackTarget();
            }
            
        }
    }

    public float getMovementSpeed() { return movementSpeed; }
    public void setMovementSpeed(float f) { movementSpeed = f; }
    public float getRotationSpeed() { return rotationSpeed; }
    public void setRotationSpeed(float f) { rotationSpeed = f; }
    public float getAttackRange() { return attackRange; }
    public void setAttackRange(float f) { attackRange = f; }
    public float getAttackCooldown() { return attackCooldown; }
    public void setAttackCooldown(float f) { attackCooldown = f; }

    void defaultEnemyStats() {
        // set attack cooldown to zero
        attackCooldownCounter = 0;

        // set state to pursue
        state = enemyState.pursue;
    }

    
    public abstract void initEnemy();
    
    void updateCounters() {
        if(attackCooldownCounter > 0) { attackCooldownCounter -= Time.deltaTime; }
    }
    void pursueTarget() {

        // check if facing target
        Vector3 localTarget = transform.InverseTransformPoint(target.transform.position);
        float angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        if(Mathf.Abs(angleToTarget) > lookAtThreshold) {
            transform.Rotate(Vector3.up * angleToTarget/Mathf.Abs(angleToTarget) * rotationSpeed * Time.deltaTime); 
            state = enemyState.pursue;
        } else {
            // snap rotation to target
            transform.Rotate(Vector3.up * angleToTarget);

            // check range to target
            float distToTarget = Vector3.Distance(transform.position, target.transform.position);
            if(distToTarget > attackRange) {
                // move towards target
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                state = enemyState.pursue;
            } else {
                // set state to attack
                state = enemyState.attack;

            }
        }
        
    }

    void attackTarget() {

        // check range to target
        float distToTarget = Vector3.Distance(transform.position, target.transform.position);
        if(distToTarget > attackRange) {
            // change state to purse
            state = enemyState.pursue;
        } else {

            // check attack cooldown
            if(attackCooldownCounter <= 0) {
                // call attack function
                performAttack();

                // set cooldown
                attackCooldownCounter = attackCooldown;
            }
        }

    }

    public abstract void performAttack();
}
