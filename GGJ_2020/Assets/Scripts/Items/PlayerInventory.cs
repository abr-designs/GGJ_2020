using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerInventory : MonoBehaviour
 {
     private Dictionary<BaseItem.itemList, int> playerInventory = new Dictionary<BaseItem.itemList, int>();

    void Start() {
        initInventory();
        // increment starting stats
        alterItemQuantity(BaseItem.itemList.oakWithBeesPlantSeed, 5);
        printInventory();
    }

     void initInventory() {
         // initialize inventory by adding empty placeholders for each item type
        playerInventory.Add(BaseItem.itemList.bramblePlantSeed, 1);
        playerInventory.Add(BaseItem.itemList.oakWithBeesPlantSeed, 0);
     }

     public void alterItemQuantity(BaseItem.itemList b, int i) {
         playerInventory[b] = i;
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
