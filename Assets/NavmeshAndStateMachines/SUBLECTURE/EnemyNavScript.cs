using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//1
using UnityEngine.AI;

//2
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyNavScript : MonoBehaviour
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

    /// <summary>
    /// All of the possible states the enemy can be in
    /// </summary>
    public enum EnemyStates { Idle, Patrol, Chase, Attack, Dead };
    private EnemyStates _currentState;

    public EnemyStates currentState
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


    [Header("Detection Settings")]
    public LayerMask visionBlockers;
    public Transform eyes;
    public float visionRange;
    public string playerTag;

    [Header("Enemy Settings")]
    public float idleWaitTime;
    private float idleWaitTimeCounter;
    public float patrolRange;
    private Vector3 patrolDestination;
    public float attackCooldown;
    private float attackCooldownCounter;
    public float lostTime;
    private float lostTimeCounter;

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
        /*
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

        switch (currentState)
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
    bool IsTargetVisible()
    {
        Debug.DrawRay(eyes.position, (target.position - eyes.position) * visionRange, Color.red, 0.05f);

        if (Vector3.Dot((target.position - eyes.position).normalized, eyes.forward) < 0)
            return false;

        if (Physics.Raycast(eyes.position, (target.position - eyes.position).normalized, out RaycastHit hitInfo, visionRange, visionBlockers))
        {
            if (hitInfo.transform.CompareTag(playerTag))
                return true;
        }
        return false;
    }

    private void Idle()
    {
        // Debug.Log(IsTargetVisible());

        if(IsTargetVisible())
        {
            currentState = EnemyStates.Chase;
            return;
        }

        idleWaitTimeCounter += Time.deltaTime;
        if (idleWaitTimeCounter >= idleWaitTime)
        {
            currentState = EnemyStates.Patrol;
            return;
        }
    }

    private void Patrol()
    {
        if(IsTargetVisible())
        {
            currentState = EnemyStates.Chase;
            return;
        }

        agent.SetDestination(patrolDestination);
        if (agent.remainingDistance <= 0.05)
        {
            currentState = EnemyStates.Idle;
            return;
        }
    }

    private void Chase()
    {
        if(!IsTargetVisible())
        {
            agent.isStopped = true;
            lostTimeCounter += Time.deltaTime;
            if (lostTimeCounter >= lostTime)
            {
                agent.isStopped = false;
                currentState = EnemyStates.Patrol;
                return;
            }
        }

        if (Vector3.Distance(target.position, this.transform.position) <= minPlayerDistance)
        {
            agent.isStopped = true;
            currentState = EnemyStates.Attack;
            return;
        }

        agent.destination = target.position;
    }

    private void Attack()
    {
        if (Vector3.Distance(target.position, this.transform.position) > maxPlayerDistance)
        {
            agent.isStopped = false;
            currentState = EnemyStates.Chase;
            return;
        }

        attackCooldownCounter += Time.deltaTime;
        if (attackCooldownCounter >= attackCooldown)
        {
            Debug.Log("ATTACKED " + target.name);
            attackCooldownCounter = 0f;
        }
    }

    private void Dead()
    {

    }
}
