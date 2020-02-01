﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : PlantBase
{
    // Start is called before the first frame update
    protected override void Init()
    {
        base.Init();
        
        transform.localScale = Vector3.zero;
    }

    //================================================================================================================//

    public override void GrowState()
    {
        if (Timer < growTime)
        {
            Timer += Time.deltaTime;
            transform.localScale = Vector3.one * growCurve.Evaluate(Timer / growTime); 
        }
        else
        {
            Timer = 0f;
            SetState(STATE.IDLE);
            transform.localScale = Vector3.one;
        }
    }

    public override void IdleState()
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
                activeSeeds[i] = null;
                continue;
            }
            
            seedTimers[i] += Time.deltaTime;
            activeSeeds[i].localScale = Vector3.one * growCurve.Evaluate( seedTimers[i] / growTime);

        }
        
    }

    public override void AttackState()
    {
        throw new System.NotImplementedException();
    }

    public override void DeathState()
    {
        throw new System.NotImplementedException();
    }
}
