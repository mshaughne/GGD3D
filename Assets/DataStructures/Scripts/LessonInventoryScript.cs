using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class LessonInventoryScript : MonoBehaviour
{
	[SerializeField]
	private List<string> pickedItems = new List<string>();

    // Update is called once per frame
    void Update()
    {
		// When we press Mouse0 (left click)
		if (Input.GetMouseButtonDown(0))
		{
			// Generate a ray (which contains only an origin and direction)
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			// show the ray visibly, going infinitely, for 5 seconds
			Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.red, 5f);

			// using this ray (which is started at an origin and pointing in a direction), storing the hit info
			// in this out variable (hitinfo), and going for this max distance (which is infinite distance)

			// tl;dr if the ray hits something, put the details in the hit variable
			if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
			{
				Debug.Log(hitInfo.transform.name);
				// if the specified script is found in the transform of the collider hit, store the script in a temp variable "script"

				// the line below works for parents, not children.
				if (hitInfo.transform.TryGetComponent<PickableItemScript> (out PickableItemScript script))
                {
					PickItem(script);
				}

				// what if we're doing it for children?
				// we can't make an "out" variable with GetComponentInChildren, so we need to do this differently.
				// first, make a variable
				/*		PickableItemScript script;	*/
				
				// then set the variable equal to the component in the child, if it exists, then check for null (which means it didn't exist)
				/*		if ((script = hitInfo.transform.GetComponentInChildren<PickableItemScript>()) != null)
							{
								// if it exists, call PickItem with the component we got
								PickItem(script);
							}
				*/
			}	// this is not the most efficient way of doing this, it is more efficient to say "if we did not hit anything, end the function.
				// I am writing it this way for the sake of example.
		}	
    }
	
    public void PickItem(PickableItemScript item)
	{
		// Add the picked item to the inventory
		pickedItems.Add(item.itemName);

		// debug-print the name of the item
		Debug.Log(item.itemName + " was added.");

		// Display all items in the inventory
		DisplayInventory();

		// Then destroy the picked item
		Destroy(item.gameObject);
	}

	public void DisplayInventory()
	{
		string output = "";

		foreach (var item in pickedItems)
		{
			output = output + item + "\n";
		}
		Debug.Log(output);
	}
}
