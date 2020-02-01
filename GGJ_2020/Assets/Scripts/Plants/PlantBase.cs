using System;
using UnityEngine;

public abstract class PlantBase : MonoBehaviour
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

    private float growTime;
    
    
    [SerializeField]
    private float attackCooldown;
    private float attackCooldownCounter;

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
        // set attack cooldown to zero
        attackCooldownCounter = 0;
        currentState = STATE.GROW;
    }
    
    //================================================================================================================//


    public abstract void GrowState();
    public abstract void IdleState();
    public abstract void AttackState();
    public abstract void DeathState();
    
    //================================================================================================================//
}
