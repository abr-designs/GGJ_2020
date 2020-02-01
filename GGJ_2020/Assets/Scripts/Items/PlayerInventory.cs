using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

class PlayerInventory : SerializedMonoBehaviour
 {
     [SerializeField]
     private Dictionary<BaseItem.itemList, int> playerInventory = new Dictionary<BaseItem.itemList, int>();

    private void Start() {
        initInventory();
        // increment starting stats
        //alterItemQuantity(BaseItem.itemList.oakWithBeesPlantSeed, 5);
        printInventory();
    }

    private void initInventory() {
         // initialize inventory by adding empty placeholders for each item type
        //playerInventory.Add(BaseItem.itemList.bramblePlantSeed, 1);
        //playerInventory.Add(BaseItem.itemList.oakWithBeesPlantSeed, 0);
     }


     public void AddSeed(BaseItem.itemList seed)
     {
         alterItemQuantity(seed, 1);
     }
     
     public GameObject GetSeed(BaseItem.itemList seed)
     {
         if (!alterItemQuantity(seed, -1))
             return null;
         switch (seed)
         {
             case BaseItem.itemList.bramblePlantSeed:
                 break;
             case BaseItem.itemList.oakWithBeesPlantSeed:
                 break;
             case BaseItem.itemList.slowingFungusPlantSeed:
                 break;
             case BaseItem.itemList.restorativeFlowerPlantSeed:
                 break;
         }
         
         throw new NotImplementedException($"{seed} has not been implemented");
     }

     /// <summary>
     /// Use bool to signify if the inventory for this item is empty
     /// </summary>
     /// <param name="b"></param>
     /// <param name="i"></param>
     /// <returns></returns>
     private bool alterItemQuantity(BaseItem.itemList b, int i) {
         
         if(!playerInventory.ContainsKey(b))
             playerInventory.Add(b, 0);
         
         playerInventory[b] = playerInventory[b] + i < 0 ? 0 : playerInventory[b] + i;

         //If we have no more of this seed, return false
         return !(playerInventory[b] <= 0);
     }

    public void printInventory() {

        Debug.Log("Printing Inventory");
        foreach(KeyValuePair<BaseItem.itemList, int> attachStat in playerInventory)
        {
            //Now you can access the key and value both separately from this attachStat as:
            Debug.Log($"Item: {attachStat.Key} = {attachStat.Value}");
        }

    }

 }
