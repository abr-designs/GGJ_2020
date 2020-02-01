﻿using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class PlantBase : MonoBehaviour, IDamageable
{
    public enum STATE
    {
        GROW,
        IDLE,
        FRUITING,
        ATTACK,
        DEATH
    }

    public enum TYPE
    {
        TREE,
        BRUSH
    }
    //================================================================================================================//

    protected static FertilityController FertilityController;

    

    [SerializeField, ReadOnly]
    private STATE currentState;
    [SerializeField]
    private TYPE plantType;
    
    [SerializeField, Range(0.1f, 50f)]
    protected float fetilityRadius = 1f;

    [SerializeField]
    private float startHealth;
    
    [SerializeField, ProgressBar(0f,nameof(startHealth),0f,1f,0f), ReadOnly, PropertyOrder(-1000)]
    private float currentHealth;
    
    [SerializeField]
    protected AnimationCurve growCurve = new AnimationCurve();

    [SerializeField]
    protected float growTime;
    
    [SerializeField]
    protected float seedGrowthTime;
    
    [SerializeField]
    protected float attackCooldown;

    [SerializeField] 
    protected GameObject seedPrefab;

    [SerializeField]
    protected Transform[] seedGrowthLocations;

    protected Transform[] activeSeeds;
    protected float[] seedTimers;
    
    
    protected float Timer;
    
    //================================================================================================================//

    private new Transform transform;

    //================================================================================================================//
    // Start is called before the first frame update
    private void Start()
    {
        if(FertilityController == null)
            FertilityController = FindObjectOfType<FertilityController>();
        
        transform = gameObject.transform;
        
        Init();
    }

    // Update is called once per frame
    private void Update()
    {
        switch (currentState)
        {
            case STATE.GROW:
                GrowState();
                break;
            case STATE.IDLE:
                IdleState();
                break;
            case STATE.FRUITING:
                FruitingState();
                break;
            case STATE.ATTACK:
                AttackState();
                break;
            case STATE.DEATH:
                DeathState();
                break;
            default:
                throw new NotImplementedException($"{currentState} not implemented.");
        }
    }

//================================================================================================================//
    protected virtual void Init()
    {
        activeSeeds = new Transform[seedGrowthLocations.Length];
        seedTimers = new float[seedGrowthLocations.Length];
        
        currentHealth = startHealth;
        
        // set attack cooldown to zero
        Timer = 0;
        //currentState = STATE.GROW;
        SetState(STATE.GROW);
    }
    
    protected virtual void SetState(STATE nextState)
    {
        currentState = nextState;
        
        switch (nextState)
        {
            case STATE.GROW:
                break;
            case STATE.IDLE:
                break;
            case STATE.FRUITING:
                break;
            case STATE.ATTACK:
                break;
            case STATE.DEATH:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextState), nextState, null);
        }
    }
    
    //================================================================================================================//


    public abstract void GrowState();
    public abstract void IdleState();
    public abstract void FruitingState();
    public abstract void AttackState();
    public abstract void DeathState();
    
    //================================================================================================================//
    public void Damage(float amount)
    {

        currentHealth -= amount;

        // Debug.Log($"Deal [{amount}] damage to [{name}]. Remaining health = [{currentHealth}]");
        
        if (currentHealth <= 0)
        {
            SetState(STATE.DEATH);

            // run death animation
            //

            // destroy/recycle enemy
            Destroy(gameObject);
            
            return;
        }
        

    }

    public void Heal(float amount)
    {
        currentHealth += amount;
    }

    //================================================================================================================//

    [FoldoutGroup("Debug Damage Tree"), Button("Tree Receive 1 Damage")]
    public void debugDamageTree() {
        Damage(1);
    }

    [FoldoutGroup("Debug Damage Tree"), Button("Kill Tree")]
    public void debugKillTree() {
        Damage(currentHealth);
    }
}
