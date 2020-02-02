using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WhompingWillow : PlantBase, IAnimationAttack
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    [SerializeField, Required]
    private Animator animator;
    
    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        
        transform.localScale = Vector3.zero;
    }

    protected override void SetState(STATE nextState)
    {
        base.SetState(nextState);
        
        switch (nextState)
        {
            case STATE.ATTACK:
                animator.SetTrigger(Attack);
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
            
            //Paints on the Fertility Controller
            FertilityController.PaintAt(transform.position + Vector3.up, fetilityRadius);
        }
        else
        {
            Timer = 0f;
            SetState(STATE.FRUITING);
            transform.localScale = Vector3.one;
        }
        
        if(EnemyInRange())
            SetState(STATE.ATTACK);
    }

    public override void IdleState()
    {
        //TODO Decide if i need to heal
        //Decide if i need to grow
        //Decide if i need to fruit
    }

    public override void FruitingState()
    {
        //Do nothing...
        for (var i = 0; i < seedGrowthLocations.Length; i++)
        {
            if (activeSeeds[i] == null)
            {
                seedTimers[i] = 0f - Random.value;
                activeSeeds[i] = Instantiate(seedPrefab, seedGrowthLocations[i].position, Quaternion.identity).transform;
                activeSeeds[i].localScale = Vector3.zero;
            }

            if (seedTimers[i] > growTime)
            {
                activeSeeds[i].GetComponent<Rigidbody>().isKinematic = false;
                activeSeeds[i].GetComponent<BoxCollider>().enabled = true;
                activeSeeds[i] = null;
                continue;
            }
            
            seedTimers[i] += Time.deltaTime;
            activeSeeds[i].localScale = Vector3.one * growCurve.Evaluate( seedTimers[i] / growTime);

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
            
            animator.SetTrigger(Attack);
        }
    }

    public override void DeathState()
    {

        Destroy(gameObject);

        throw new System.NotImplementedException();
    }

    public void AnimationAttack()
    {
        Debug.Break();
    }
}
