// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Chase, 
        Investigate,
        Attack
    }
    public EnemyState currentState;

    [Header("References")] [SerializeField]
    private GameObject player;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] private Rigidbody rb;
    
    [Header("Patrol")]
    [SerializeField]
    float minNextPointDistance = 5;
    [SerializeField] float maxNextPointDistance = 30;
    [SerializeField] float patrolMaxWaitTime = 7f;
    [SerializeField] float patrolMinWaitTime = 1f;
    float currentPatrolWaitTime = 0;
    bool hasSelectedNextPoint = false;
    private bool isPatrolWaiting = false;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                SelectAndMoveToNewPoint();
                break;
            
            case EnemyState.Chase:

                break;
            
            case EnemyState.Investigate:
                
                break;
            
            case EnemyState.Attack:
                
                break;
            
            default:
                currentState = EnemyState.Patrol;
                break;
        }
    }


    //Patrol Logic

    void SelectAndMoveToNewPoint()
    {
        if (!hasSelectedNextPoint)
        {
            Vector3 nextPoint = new Vector3(transform.position.x + Random.Range(-maxNextPointDistance,maxNextPointDistance), 0, transform.position.z + Random.Range(-maxNextPointDistance,maxNextPointDistance));
            hasSelectedNextPoint = true;
            agent.SetDestination(nextPoint);
            Debug.Log("Selected Next Point");
            
        }
        WaitOnPatrolPoint();
    }

    void WaitOnPatrolPoint()
    {
        if (agent.velocity.magnitude < 0.1f)
        {
            if (!isPatrolWaiting)
            {
                currentPatrolWaitTime = Random.Range(patrolMaxWaitTime, patrolMaxWaitTime);
                isPatrolWaiting = true;
                Debug.Log("Set Waiting time of: " + currentPatrolWaitTime);
            }
            else
            {
                currentPatrolWaitTime -= Time.deltaTime;
                if (currentPatrolWaitTime <= 0)
                {
                    Debug.Log("Waiting for: " + currentPatrolWaitTime);
                    hasSelectedNextPoint = false;
                    isPatrolWaiting = false;
                }
            }
            
        }
    }
    
}
