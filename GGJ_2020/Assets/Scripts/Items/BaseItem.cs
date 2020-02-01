using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem {

    public enum itemList {
        bramblePlantSeed,
        oakWithBeesPlantSeed,
        slowingFungusPlantSeed,
        restorativeFlowerPlantSeed
    }

    public string title;
    public GameObject prefab;
    public Dictionary<string, int> stats = new Dictionary<string, int>();
    
    public BaseItem(string title) {
        this.title = title;
        // this.prefab = prefab;
    }

    public string getTitle() { return title; }
    public GameObject getPrefab() { return prefab; }

}