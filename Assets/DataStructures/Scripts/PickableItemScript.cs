using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItemScript : MonoBehaviour
{
	// The name that we will show on the inventory
	public string itemName;

	// You can use [SerializeField] to get a variable to appear in editor
	// without making that variable public (we will not be using this example)
	[SerializeField]
	string exampleName;
}
