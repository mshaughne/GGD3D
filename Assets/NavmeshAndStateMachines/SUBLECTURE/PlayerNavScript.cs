using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerNavScript : MonoBehaviour
{
	/// <summary>
	/// The agent that controls the character's movement.
	/// </summary>
	public NavMeshAgent agent;

	// Start is called before the first frame update
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetMouseButton(0))
		{
			GetPlayerDestination();
		}
	}

	/// <summary>
	/// Calculates where the player is clicking and sets the agent destination to that point.
	/// </summary>
	private void GetPlayerDestination()
	{
		// will get us a ray coming from the camera in the direction that the player clicked
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		//if the mouse click lands on a physical object
		if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity))
		{
			// set the point of contact as the player's destination
			agent.destination = hitInfo.point;
		}
	}
}
