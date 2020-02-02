using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : PlantBase
{
    [SerializeField]
    int numShrubs = 10;
    public GameObject shrubPrefab;

    private GameObject[] shrubs;
    // Start is called before the first frame update

    protected override void Init()
    {
        base.Init();
        shrubs = new GameObject[numShrubs]; 
        for(int i =0; i <numShrubs; i ++)
        {
   
            Vector2 PlanarCoordinate = Random.insideUnitCircle * fetilityRadius;
            Vector3 position = new Vector3(transform.position.x + PlanarCoordinate.x, transform.position.y, transform.position.z+PlanarCoordinate.y);
            float rotation = Random.Range(0, 359);
            shrubs[i] = Instantiate(shrubPrefab, position, Quaternion.Euler(Vector3.up *rotation));
        }
        transform.localScale = Vector3.zero;
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
            FertilityController.PaintAt(transform.position + Vector3.up, fetilityRadius);
            
            var shrubTotal = Mathf.RoundToInt(numShrubs * CurrentGrowth);
            for (var i = 0; i < numShrubs; i++)
            {
                shrubs[i].SetActive(i <= shrubTotal);
                shrubs[i].transform.localScale = Vector3.one * CurrentGrowth;
            }
        }
        else
        {
            Timer = 0f;
            SetState(STATE.FRUITING);
            transform.localScale = Vector3.one;
        }
    }

    public override void IdleState()
    {
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
        throw new System.NotImplementedException();
    }

    public override void DeathState()
    {

        Destroy(gameObject);

        throw new System.NotImplementedException();
    }
}
