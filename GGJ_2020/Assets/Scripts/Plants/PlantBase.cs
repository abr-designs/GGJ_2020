using System;
using UnityEngine;

public abstract class PlantBase : MonoBehaviour, IDamageable
{
    public enum STATE
    {
        GROW,
        IDLE,
        ATTACK,
        DEATH
    }
    
    //================================================================================================================//

    private STATE currentState;

    private float health;
    
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

    private new Transform transform;

    //================================================================================================================//
    // Start is called before the first frame update
    private void Start()
    {
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
        
        
        // set attack cooldown to zero
        Timer = 0;
        //currentState = STATE.GROW;
        SetState(STATE.GROW);
    }
    
    protected void SetState(STATE nextState)
    {
        currentState = nextState;
        
        switch (nextState)
        {
            case STATE.GROW:
                break;
            case STATE.IDLE:
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
    public abstract void AttackState();
    public abstract void DeathState();
    
    //================================================================================================================//
    public void Damage(float amount)
    {
        if(health < 0)
            return;
        
        health -= amount;
    }

    public void Heal(float amount)
    {
        health += amount;
    }
}
