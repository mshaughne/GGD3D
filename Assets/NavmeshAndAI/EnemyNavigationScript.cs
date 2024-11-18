using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1
using UnityEngine.AI;

//2
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavigationScript : MonoBehaviour
{
	//3
	/// <summary>
	/// The transform this enemy will chase.
	/// </summary>
	public Transform target;
	//4
	/// <summary>
	/// The agent that controls this object's navigation
	/// </summary>
	public NavMeshAgent agent;

	//8
	public float minPlayerDistance;
	public float maxPlayerDistance;

	// Start is called before the first frame update
	void Start()
	{
		//5
		// get the navmeshagent component on this object
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
	{
		// we usually shouldn't put a lot of stuff in update, but we'll be putting it here

		//6
		// if there is a target
		if (target != null)
		{
			//9
			if (Vector3.Distance(target.position, this.transform.position) <= minPlayerDistance)
				agent.isStopped = true;
			else if (Vector3.Distance(target.position, this.transform.position) > maxPlayerDistance)
				agent.isStopped = false;

			//7
			// move towards the target
			agent.destination = target.position;
		}
	}
}
