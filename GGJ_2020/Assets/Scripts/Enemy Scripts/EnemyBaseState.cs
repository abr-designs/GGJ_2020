﻿using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Recycling;

public abstract class EnemyBaseState : MonoBehaviour, IDamageable
{
    
public enum enemyState {
    idle,
    followWaypoint,
    pursueTarget,
    attack,
    dying
};

[SerializeField]
private enemyState state;

// waypoint stats
public RobotPathway robotPathway;

Waypoint currentWaypoint; 

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
            // check if pathway is null
            if(robotPathway != null) {
                advanceOnWaypoint();
            } else {
                // change state to idle
                state = enemyState.idle;
            }
            searchForTarget();
        } else {
            if(state == enemyState.pursueTarget) {
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

    // get set pathway stats
    public RobotPathway getPathway() { return robotPathway; }
    public void setPathway(RobotPathway path) { robotPathway = path; }

    void defaultEnemyStats() {
        // set attack cooldown to zero
        attackCooldownCounter = 0;

        // set state to pursue
        // state = enemyState.pursueTarget;
        state = enemyState.followWaypoint;

        // define initial waypoint
        if(robotPathway != null) {
            currentWaypoint = robotPathway.pathway[0];
        }
        
    }

    // each enemy will need to be loaded with their specifc stats based on the type of enemy
    public abstract void initEnemy();
    
    // update counters being tracked for this enemy
    void updateCounters() {
        if(attackCooldownCounter > 0) { attackCooldownCounter -= Time.deltaTime; }
    }

    public void advanceOnWaypoint() {

        // determine position of target Waypoint
        Debug.Log(currentWaypoint);

        // check if facing target
        Vector3 localTarget = transform.InverseTransformPoint(currentWaypoint.gameObject.transform.position);
        float angleToTarget = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg;
        if(Mathf.Abs(angleToTarget) > lookAtThreshold) {
            transform.Rotate(Vector3.up * angleToTarget/Mathf.Abs(angleToTarget) * rotationSpeed * Time.deltaTime); 
            // state = enemyState.pursueTarget;
        } else {
            // snap rotation to target
            transform.Rotate(Vector3.up * angleToTarget);

            // check range to target
            float distToTarget = Vector3.Distance(transform.position, localTarget);
            float waypointDistThreshold = 2.0f;
            if(distToTarget > waypointDistThreshold) {
                // move towards target
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                // state = enemyState.pursueTarget;
            } else {
                // increment to next waypoint
                int waypointIndex = robotPathway.pathway.IndexOf(currentWaypoint);
                waypointIndex += 1;
                currentWaypoint = robotPathway.pathway[waypointIndex];
            }
        }

    }

    public void searchForTarget() {

        // check distance to a target plant
        
        // search a target forlder for any gameobject
        GameObject targetsParent = GameObject.Find("Trees").gameObject;
        // select first child object from targetsParent
        foreach(Transform child in targetsParent.transform) {

            // check distance
            float dist = Vector3.Distance(child.transform.position, transform.position);
            float distThreshold = 10.0f; // move to a class variable
            if(dist < distThreshold) {
                setTarget(child.gameObject);
                // set state to pursueTarget
                state = enemyState.pursueTarget;
            }
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
            state = enemyState.pursueTarget;
        } else {
            // snap rotation to target
            transform.Rotate(Vector3.up * angleToTarget);

            // check range to target
            float distToTarget = Vector3.Distance(transform.position, target.transform.position);
            if(distToTarget > attackRange) {
                // move towards target
                transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
                state = enemyState.pursueTarget;
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
            state = enemyState.pursueTarget;
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

        Debug.Log($"Deal [{amount}] damage to [{name}]. Remaining health = [{health}]");

        if(health <= 0) {
            state = enemyState.dying;

            // run death animation
            //

            // destroy/recycle enemy
            Destroy(gameObject);
        }
    }

    public void Heal(float amount)
    {
        health += amount;
    }


    [FoldoutGroup("Debug Damage Enemy"), Button("Enemy Receive 1 Damage")]
    public void debugDamageEnemy() {
        Damage(1);
    }

    [FoldoutGroup("Debug Damage Enemy"), Button("Kill Enemy")]
    public void debugKillEnemy() {
        Damage(health);
    }
}
