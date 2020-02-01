using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<EnemyBaseState> enemies { get; private set; }
    public List<PlantBase> plants{ get; private set; }
    public List<EnemySpawnController> spawners{ get; private set; }

    private GameObject playerGameObject;
    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
        playerGameObject = playerInventory.gameObject;
    }

    //================================================================================================================//
    
    #region Registration of Objects
    
    public void RegisterEnemy(EnemyBaseState enemy)
    {
        if(enemies== null)
            enemies = new List<EnemyBaseState>();
        
        enemies.Add(enemy);
    }
    public void UnRegisterEnemy(EnemyBaseState enemy)
    {
        enemies?.Remove(enemy);
    }
    
    public void RegisterPlant(PlantBase plant)
    {
        if(plants== null)
            plants = new List<PlantBase>();
        
        plants.Add(plant);
    }
    public void UnRegisterPlant(PlantBase plant)
    {
        plants?.Remove(plant);
    }
    
    public void RegisterPlant(EnemySpawnController enemySpawnController)
    {
        if(spawners== null)
            spawners = new List<EnemySpawnController>();
        
        spawners.Add(enemySpawnController);
    }
    public void UnRegisterPlant(EnemySpawnController enemySpawnController)
    {
        spawners?.Remove(enemySpawnController);
    }
    #endregion //Registration of Objects
    
    //================================================================================================================//
}
