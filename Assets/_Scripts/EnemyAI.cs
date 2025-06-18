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
    [SerializeField] GunProperties turretProperties;
    
    
    [Header("Patrol State Variables")]
    [SerializeField]
    float minNextPointDistance = 5;
    [SerializeField] float maxNextPointDistance = 30;
    [SerializeField] float patrolMaxWaitTime = 7f;
    [SerializeField] float patrolMinWaitTime = 1f;
    float currentPatrolWaitTime = 0;
    bool hasSelectedNextPoint = false;
    private bool isPatrolWaiting = false;
    //Turret rotate
    [SerializeField] private Transform turretTransform;
    [SerializeField] private float minTurretRotationWaitTime = 1f;
    [SerializeField] private float maxTurretRotationWaitTime = 4f;
    float currentTurretRotationWaitTime = 0;
    private bool isTurretRotated = true;
    private Vector3 turretTargetRotation;
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
                RotateCanon();
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
                currentPatrolWaitTime = Random.Range(patrolMinWaitTime, patrolMaxWaitTime);
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

    void RotateCanon()
    {
        if (isTurretRotated)
        {
            turretTargetRotation =  new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
            currentTurretRotationWaitTime = Random.Range(minTurretRotationWaitTime, maxTurretRotationWaitTime);
            isTurretRotated = false;
        }
        else
        {
            Vector3 direction = (turretTargetRotation - turretTransform.localPosition);
            
            float targetAngle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
            float currentAngle = turretTransform.localEulerAngles.y;
            
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turretProperties.rotateSpeed * Time.deltaTime);

            turretTransform.localRotation = Quaternion.Euler(0f, newAngle, 0f);
            if (Mathf.Abs(Mathf.DeltaAngle(currentAngle, targetAngle)) < 1f)
            {
                currentTurretRotationWaitTime -= Time.deltaTime;
                if (currentTurretRotationWaitTime <= 0) isTurretRotated = true;
            }
        }
    }
    
}
