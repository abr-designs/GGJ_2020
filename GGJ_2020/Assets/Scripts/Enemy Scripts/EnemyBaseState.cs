using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
// using Unity.Mathf;

public abstract class EnemyBaseState : MonoBehaviour, IDamageable
{
    
public enum enemyState {
    idle,
    pursue,
    attack,
    dying
};

private enemyState state;
[SerializeField]
protected GameObject target;
// movement stats
private float movementSpeed = 5.0f;
private float lookAtThreshold = 2.5f;

private float rotationSpeed = 50.0f;
// health stats
protected float health;
private float defense;
// attack stats
private float attackRange = 2.5f;
private float attackCooldown;
private float attackCooldownCounter;
private float attackDamage;
// drop table stats


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

        if(target == null) {
            searchForTarget();
        } else {
            if(state == enemyState.pursue) {
                pursueTarget();
            } else if(state == enemyState.attack) {
                attackTarget();
            }
        }
    }

    // get set movement stats
    public float getMovementSpeed() { return movementSpeed; }
    public void setMovementSpeed(float f) { movementSpeed = f; }
    public float getRotationSpeed() { return rotationSpeed; }
    public void setRotationSpeed(float f) { rotationSpeed = f; }
    // get set health stats
    public float getHealth() { return health; }
    public void setHealth(float f) { health = f; }
    public float getDefense() { return defense; }
    public void setDefense(float f) { defense = f; }

    // get set attack stats
    public float getAttackRange() { return attackRange; }
    public void setAttackRange(float f) { attackRange = f; }
    public float getAttackCooldown() { return attackCooldown; }
    public void setAttackCooldown(float f) { attackCooldown = f; }
    public float getAttackDamage() { return attackDamage; }
    public void setAttackCDamage(float f) { attackDamage = f; }

    void defaultEnemyStats() {
        // set attack cooldown to zero
        attackCooldownCounter = 0;

        // set state to pursue
        state = enemyState.pursue;
    }

    // each enemy will need to be loaded with their specifc stats based on the type of enemy
    public abstract void initEnemy();
    
    // update counters being tracked for this enemy
    void updateCounters() {
        if(attackCooldownCounter > 0) { attackCooldownCounter -= Time.deltaTime; }
    }

    public void searchForTarget() {

        // placeholder
        // search a target forlder for any gameobject
        GameObject targetsParent = GameObject.Find("Trees").gameObject;
        // select first child object from targetsParent
        foreach(Transform child in targetsParent.transform) {
            setTarget(child.gameObject);
            break;
        }
    }

    public void setTarget(GameObject g) {
        target = g;
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

    // enemy will received damage from an attack minus its defense stat
    public void receiveDamage(float f) {
        
        
    }

    // abstract function to perform a specific attack base on the type of enemy
    public abstract void performAttack();

    // abstract function to call items to be dropped based on this enemies drop tables
    // public abstract void dropLoot();

    public void Damage(float amount)
    {
        health -= amount - defense;

        if(health <= 0) {
            state = enemyState.dying;
        }
    }

    public void Heal(float amount)
    {
        health += amount;
    }
}
