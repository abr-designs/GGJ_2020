using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class BaseItem : MonoBehaviour
{

    public enum itemList {
        BASE_SEED,
        WILLOW_SEED,
        BUSH_SEED,
        FUNGUS_SEED
    }

    [SerializeField, Required]
    protected GameObject prefab;
    //public Dictionary<string, int> stats = new Dictionary<string, int>();
    
    /*public BaseItem(string title) {
        this.title = title;
        // this.prefab = prefab;
    }*/

    //public string getTitle() { return title; }
    //public GameObject getPrefab() { return prefab; }

}