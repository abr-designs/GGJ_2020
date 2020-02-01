using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public abstract class ItemPickup : MonoBehaviour
{
	[SerializeField]
	private string targetTag;
	private new BoxCollider collider { get; set; }

	private void Start()
	{
		collider = GetComponent<BoxCollider>();
		
		if(string.IsNullOrEmpty(targetTag))
			throw new NullReferenceException($"{nameof(targetTag)} is not set on {gameObject.name}");

		if (!collider.isTrigger)
			throw new ArgumentException($"Collider should be trigger on {gameObject.name}");

		Init();
	}


	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag(targetTag))
		{
			PickupItem(other.gameObject);
		}
	}
	
	protected abstract void Init();


	protected abstract void PickupItem(GameObject collidedObject);

}
