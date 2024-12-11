using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//1
// add the AI components
using UnityEngine.AI;

//2
// make the script require a NavMeshAgent component
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
	//3
	/// <summary>
	/// The agent that controls the character's movement
	/// </summary>
	public NavMeshAgent agent;

	public float health = 100;

	// Start is called before the first frame update
	void Start()
	{
		//4
		// find the nav mesh agent and assign it
		agent = GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update()
	{
        //9
        // when the left click is held
        if(Input.GetMouseButton(0))
			//10
			GetPlayerDestination();
		if(Input.GetKeyDown(KeyCode.F))
			SpherecastCheck();
		if (Input.GetMouseButton(1))
			Turning();
	}

	//5
	/// <summary>
	/// Calculates where the player is clicking and sets the agent destination there
	/// </summary>
	private void GetPlayerDestination()
	{
		//6
		// will get us a ray coming from the cam in the direction the player clicked
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		//7
		// if the click lands on a physical object
		if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity))
		{
			//8
			// set the point of contact with the object as the player's destination
			agent.destination = hitInfo.point;
		}
	}

	public void Damage(float damage)
	{
		health -= damage;

		if (health <= 0)
		{
			//Instantiate(particle, this.transform.position, this.transform.rotation);
			Destroy(this.gameObject);
		}
	}

	void SpherecastCheck()
	{
		Debug.DrawRay(transform.position, transform.forward*5, Color.yellow, 5);

		if (Physics.SphereCast(transform.position, 3f, transform.forward, out RaycastHit hit))
		{
			Debug.Log(hit);
		}
	}

	void Turning()
	{
		Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

		// if the click lands on a physical object
		if (Physics.Raycast(mouseRay, out RaycastHit hitInfo, Mathf.Infinity))
		{
			// rotate the character to face the point
			this.transform.LookAt(hitInfo.point);
		}
	}
}
