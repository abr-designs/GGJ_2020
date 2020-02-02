using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class HomeBaseTree : PlantBase, IAnimationAttack
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    //================================================================================================================//

    // [SerializeField, Required]
    // private Animator animator;
    
    //================================================================================================================//

    // // reference static
    // protected static GameManager gm;

    // // Start is called before the first frame update
    // void Start()
    // {
    //     if(gm == null) {
    //         gm = FindObjectOfType<GameManager>();
    //     }
    // }
    
    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        
        transform.localScale = Vector3.zero;

        // pass this tree to the gm to set the stage home base tree
        GameManager.setStageBaseTree(gameObject);
    }

    protected override void SetState(STATE nextState)
    {
        base.SetState(nextState);
        
        switch (nextState)
        {
            case STATE.ATTACK:
                // animator.SetTrigger(Attack);
                break;
        }
    }

    //================================================================================================================//

    public override void GrowState()
    {
        if (Timer < growTime)
        {
            Timer += Time.deltaTime;
            transform.localScale = Vector3.one * growCurve.Evaluate(Timer / growTime);

            currentHealth = startHealth * growCurve.Evaluate(Timer / growTime);
            
            //Paints on the Fertility Controller
            FertilityController.PaintAt(transform.position + Vector3.up, fertilityRadius);
        }
        else
        {
            Timer = 0f;
            SetState(STATE.FRUITING);
            transform.localScale = Vector3.one;
            currentHealth = startHealth;
        }
        
        if(EnemyInRange())
            SetState(STATE.ATTACK);
    }

    public override void IdleState()
    {
        if (currentHealth < startHealth)
        {
            Timer = growTime * growCurve.Evaluate(currentHealth / startHealth);
            
            SetState(STATE.GROW);
        }
        else if(isFruiting)
        {
            SetState(STATE.FRUITING);
        }

    }

    public override void FruitingState()
    {
        if (!isFruiting)
        {
            SetState(STATE.IDLE);
            return;
        }
        
        //Do nothing...
        for (var i = 0; i < seedGrowthLocations.Length; i++)
        {
            if (activeSeeds[i] == null)
            {
                seedTimers[i] = 0f - Random.value;
                activeSeeds[i] = Instantiate(seedPrefab, seedGrowthLocations[i].position, Quaternion.identity, GameManager.pickupSeedsContainer).transform;
                activeSeeds[i].localScale = Vector3.zero;
            }

            if (seedTimers[i] > seedGrowthTime)
            {
                activeSeeds[i].GetComponent<Rigidbody>().isKinematic = false;
                activeSeeds[i].GetComponent<BoxCollider>().enabled = true;
                activeSeeds[i] = null;
                continue;
            }
            
            seedTimers[i] += Time.deltaTime;
            activeSeeds[i].localScale = Vector3.one * growCurve.Evaluate( seedTimers[i] / seedGrowthTime);

        }
        
        
    }

    public override void AttackState()
    {
        if (Timer < attackCooldown)
        {
            Timer += Time.deltaTime;
        }
        else
        {
            Timer = 0f;

            if (!EnemyInRange())
            {
                SetState(STATE.IDLE);
                return;
            }
            
            // animator.SetTrigger(Attack);
        }
    }

    public override void DeathState()
    {

        Destroy(gameObject);

        throw new System.NotImplementedException();
    }
    
    //================================================================================================================//


    public void AnimationAttack()
    {
        var enemies = GameManager.enemies;

        if (enemies == null || enemies.Count == 0)
            return;

        enemies.Where(e => Vector3.Distance(e.transform.position, transform.position) <= attackRange * CurrentGrowth)
            .ForEach(e => e.Damage(attackDamage));
    }
    
    //================================================================================================================//

    public override void Damage(float amount)
    {
        
        transform.localScale = Vector3.one * growCurve.Evaluate(currentHealth / startHealth);
        
        base.Damage(amount);

    }
}
