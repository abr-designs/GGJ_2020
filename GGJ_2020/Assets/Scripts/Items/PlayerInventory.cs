using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

class PlayerInventory : SerializedMonoBehaviour
{
    //================================================================================================================//

    [SerializeField, DisableInPlayMode] 
    private bool isDebug;

    //================================================================================================================//

    [ReadOnly]
    public BaseItem.itemList? currentlySelected;

    //================================================================================================================//


    [SerializeField, ReadOnly]
    private Dictionary<BaseItem.itemList, int> playerInventory = new Dictionary<BaseItem.itemList, int>();

    [SerializeField, DisableInPlayMode, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.Foldout)]
    private Dictionary<BaseItem.itemList, GameObject> ammoLibrary =
        new Dictionary<BaseItem.itemList, GameObject>
        {
            {
                BaseItem.itemList.BASE_SEED, null
            },
            {
                BaseItem.itemList.WILLOW_SEED, null
            },
            {
                BaseItem.itemList.BUSH_SEED, null
            },
            {
                BaseItem.itemList.FUNGUS_SEED, null
            },
        };

    //================================================================================================================//

    [FoldoutGroup("Held Objects"), SerializeField]
    private Dictionary<BaseItem.itemList, GameObject> itemHoldLibrary =
        new Dictionary<BaseItem.itemList, GameObject>
        {
            {
                BaseItem.itemList.BASE_SEED, null
            },
            {
                BaseItem.itemList.WILLOW_SEED, null
            },
            {
                BaseItem.itemList.BUSH_SEED, null
            },
            {
                BaseItem.itemList.FUNGUS_SEED, null
            },
        };
    [FoldoutGroup("Held Objects"), ]
    public Vector3 localHoldPosition = Vector3.zero;
    [FoldoutGroup("Held Objects"), ]
    public Vector3 localHoldRotation = Vector3.zero;



    //================================================================================================================//

    private void Start()
    {
        initInventory();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
            ChooseNextItem();
    }

    //================================================================================================================//

    
    private void initInventory()
    {
        InitShownObjects();

        if (isDebug)
        {
            currentlySelected = BaseItem.itemList.BASE_SEED;
            for(var i = 0; i < 3; i++)
                AddSeed((BaseItem.itemList)i);
            ShowSelectedObject();
        }
        else
            currentlySelected = null;
    }

    public void ChooseNextItem()
    {
        //If there's no inventory dont bother
        if(playerInventory.Count == 0)
            return;

        //If we have something currently selected, we'll try and select the next thing
        if (currentlySelected.HasValue)
        {
            var value = (int)currentlySelected.Value;

            while (true)
            {
                value++;
                if (value > 3)
                    value = 0;

                //If we've used our last seed, don't show anything
                if (value == (int) currentlySelected.Value && playerInventory[currentlySelected.Value] <= 0)
                {
                    currentlySelected = null;
                    Debug.Log($"currentlySelected = null");
                    ShowSelectedObject();
                    return;
                    
                }
                //If we hit the start value again, then just leave
                if (value == (int) currentlySelected.Value)
                    return;

                if (!playerInventory.ContainsKey((BaseItem.itemList) value)) 
                    continue;

                if (playerInventory[(BaseItem.itemList) value] <= 0) 
                    continue;
                
                currentlySelected = (BaseItem.itemList) value;
                Debug.Log($"currentlySelected = {currentlySelected.Value}");
                ShowSelectedObject();
                return;

            }
        }

        
        //If nothing is currently selected, then we'll pick which ever is first
        foreach (var item in playerInventory.Where(item => item.Value > 0))
        {
            currentlySelected = item.Key;

            ShowSelectedObject();
            return;
        }
    }

    public void AddSeed(BaseItem.itemList seed)
    {
        alterItemQuantity(seed, 1);

        //If we've just picked up something with no previously selected, so choose that as new seed
        if (currentlySelected.HasValue) 
            return;
        
        currentlySelected = seed;
        ShowSelectedObject();
    }

    public bool TryGetSeed(out GameObject prefab)
    {
        prefab = null;
        
        if (!currentlySelected.HasValue)
            return false;

        var seed = currentlySelected.Value;

        if (!isDebug)
        {
            if (playerInventory.ContainsKey(seed) && playerInventory[seed] == 0)
            {
                return false;
            }
            
            
            alterItemQuantity(seed, -1);
            
            if (playerInventory[seed] == 0)
            {
                ChooseNextItem();
                ShowSelectedObject();
            }
        }

        prefab = ammoLibrary[seed];

        return true;
    }

    public void ClearInventory()
    {
        playerInventory = new Dictionary<BaseItem.itemList, int>();
    }

    /// <summary>
    /// Use bool to signify if the inventory for this item is empty
    /// </summary>
    /// <param name="b"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    private void alterItemQuantity(BaseItem.itemList b, int i)
    {

        if (!playerInventory.ContainsKey(b))
            playerInventory.Add(b, 0);

        playerInventory[b] = playerInventory[b] + i < 0 ? 0 : playerInventory[b] + i;
    }
    
    //================================================================================================================//

    private void ShowSelectedObject()
    {
        foreach (var o in itemHoldLibrary.Where(o => o.Value != null))
        {
            o.Value.SetActive(currentlySelected.HasValue && (o.Key == currentlySelected.Value));
        }
    }

    private void InitShownObjects()
    {
        foreach (var o in itemHoldLibrary.Where(o => o.Value != null))
        {
            o.Value.transform.localPosition = localHoldPosition;
            o.Value.transform.localRotation = Quaternion.Euler(localHoldRotation);
            
            o.Value.SetActive(false);
        }
    }


}
