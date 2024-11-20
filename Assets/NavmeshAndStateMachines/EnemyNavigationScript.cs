using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
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
	// the distance at which the enemy will stop following
	public float minPlayerDistance;
	// the distance at which the enemy will continue following
	public float maxPlayerDistance;

	//a1
	/// <summary>
	/// All of the possible states the enemy can be in
	/// </summary>
	public enum EnemyStates { Idle, Patrol, Chase, Attack, Dead };
	private EnemyStates _currentState;
	//a2
	/// <summary>
	/// The current state of the enemy;
	/// </summary>
	public EnemyStates currentState
	//a7
	{
		get { return _currentState; }
		set
		{
			if (value == _currentState)
				return;
			if (value == EnemyStates.Idle)
				idleWaitTimeCounter = 0;
			else if (value == EnemyStates.Patrol)
				// calculate a new patrol destination by taking the current pos and adding a random value in the patrol range
				patrolDestination = this.transform.position + new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange));
			else if (value == EnemyStates.Attack)
				attackCooldownCounter = 0;

			// set the current state
			_currentState = value;
		}
	}

	//a5
	[Header("Detection Settings")]
	/// <summary>
	/// layers that block the enemy's vision
	/// </summary>
	public LayerMask visionBlockers;
	/// <summary>
	/// a transform showing where the enemy's eyes are
	/// </summary>
	public Transform eyes;
	/// <summary>
	/// maximum distance where the enemy can see the player
	/// </summary>
	public float visionRange;
	/// <summary>
	/// The tag objects have when they are a part of the player
	/// </summary>
	public string playerTag;

	//a6
	[Header("Enemy Settings")]
	/// <summary>
	/// how long the enemy should be idle
	/// </summary>
	public float idleWaitTime;
	/// <summary>
	/// the counter for idleWaitTime
	/// </summary>
	private float idleWaitTimeCounter = 0;
	/// <summary>
	/// how far the patrolling enemy should move
	/// </summary>
	public float patrolRange;
	/// <summary>
	/// where the enemy is patrolling to
	/// </summary>
	private Vector3 patrolDestination;
	/// <summary>
	/// how often the enemy can attack
	/// </summary>
	public float attackCooldown;
	/// <summary>
	/// the counter for attackCooldown
	/// </summary>
	private float attackCooldownCounter = 0;

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
		/* OLD, NON-STATE MACHINE CODE
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
		}*/

		//a4
		// New State Machine code
		// Based on the current state, execute the proper code
		switch(currentState)
		{
			case EnemyStates.Idle:
				Idle();
				break;
			case EnemyStates.Patrol:
				Patrol();
				break;
			case EnemyStates.Chase:
				Chase();
				break;
			case EnemyStates.Attack:
				Attack();
				break;
			case EnemyStates.Dead:
				Dead();
				break;
			default:
				Debug.Log("State not recognized");
				break;
		}
	}

	// State Machines

	//a8
	/// <summary>
	/// Tells us whether the target object is visible to this enemy.
	/// </summary>
	/// <returns>True if a raycast casted from the eyes hits a collider with the tag "player".</returns>
	bool IsTargetVisible()
	{
		Debug.DrawRay(eyes.position, (target.position - eyes.position)*visionRange, Color.red, 0.05f);

		// if the player is behind the enemy, don't bother checking
		// the dot product of 2 normal vectors is 1 if parallel, 0 if perpendicular, and -1 if opposite
		if (Vector3.Dot((target.position - eyes.position).normalized, eyes.forward) < 0)
			return false;

		// cast a ray from the enemy's eyes, in the direction of the player, making sure it goes through glass and enemies, and that it goes no longer than maxPlayerDistance
		if (Physics.Raycast(eyes.position, (target.position - eyes.position).normalized, out RaycastHit hitInfo, visionRange, visionBlockers))
		{
			if(hitInfo.transform.CompareTag(playerTag))
				return true;
		}
		return false;
	}

	//a3
	private void Idle()
	{
		//a9
		//Debug.Log(IsTargetVisible());

		//a10 ^ comment out the debug
		if (IsTargetVisible())
		{
			currentState = EnemyStates.Chase;
			return;
		}

		//a12
		idleWaitTimeCounter += Time.deltaTime;
		if(idleWaitTimeCounter >= idleWaitTime)
		{
			currentState = EnemyStates.Patrol;
			return;
		}
	}

	private void Patrol()
	{
		//a11
		if (IsTargetVisible())
		{
			currentState = EnemyStates.Chase;
		}

		//a13
		agent.SetDestination(patrolDestination);
		if (agent.remainingDistance <= 0.05)
		{
			currentState = EnemyStates.Idle;
			return;
		}
	}

	private void Chase()
	{
		//a14
		if (Vector3.Distance(target.position, this.transform.position) <= minPlayerDistance)
		{
			agent.isStopped = true;
			currentState = EnemyStates.Attack;
			return;
		}
		// move towards the target
		agent.destination = target.position;
	}

	private void Attack() 
	{
		//a15
		if (Vector3.Distance(target.position, this.transform.position) > maxPlayerDistance)
		{
			agent.isStopped = false;
			currentState = EnemyStates.Chase;
			return;
		}
		
		//a16
		attackCooldownCounter += Time.deltaTime;

		//a17
		if(attackCooldownCounter >= attackCooldown)
		{
			Debug.Log("ATTACKED " + target.name);
			attackCooldownCounter = 0;
		}
	}

	private void Dead()
	{

	}
}