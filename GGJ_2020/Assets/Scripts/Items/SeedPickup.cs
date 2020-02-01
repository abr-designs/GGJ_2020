using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPickup : ItemPickup
{
	private static PlayerInventory PlayerInventory;
	
	[SerializeField]
	private BaseItem.itemList itemType;
	protected override void Init()
	{
		if (PlayerInventory == null)
			PlayerInventory = FindObjectOfType<PlayerInventory>();
	}

	protected override void PickupItem(GameObject player)
	{
		PlayerInventory.AddSeed(itemType);
		Destroy(gameObject);
	}
}
