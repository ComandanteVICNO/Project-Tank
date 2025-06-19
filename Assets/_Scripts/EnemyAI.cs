// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrol,
        Alert,
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
    [SerializeField] private TMP_Text text;
    [SerializeField] private EnemyAttack enemyAttackScript;
    [SerializeField] private Light spotLight;
    [SerializeField] private Color32 normalColor;
    [SerializeField]private Color32 alertedColor;
    [SerializeField] private Color32 detectedColor;
    
    
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

    [Header("Alert State Variables")]
    [SerializeField] float engageDelayTime = 2f;
    float currentEngageDelayTime = 0;
    
    [Header("Player Detection Variables")] 
    [SerializeField]private int rayCount = 7;
    [SerializeField] private float viewAngle = 60f;
    [SerializeField] private float viewDistance = 50f;

    [Header("Chase State Variables")] 
    [SerializeField] private float distanceToAttackPlayer = 15f;

    [Header("Attack state Variables")] 
    [SerializeField] private float distanceToBreakAttack = 20f;

    [Header("Investigate State Variables")] 
    [SerializeField] private float minInvestigationTime = 3f;
    [SerializeField] private float maxInvestigationTime = 7f;
    float investigationTime = 0;
    float currentInvestigationTime = 0;
    
    
    [SerializeField] Vector3 lastPlayerPosition = Vector3.zero;
    
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemyAttackScript = GetComponent<EnemyAttack>();
        
        normalColor = spotLight.color;
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                SelectAndMoveToNewPoint();
                RotateCanon();
                if (CanSeePlayerInCone()) currentState = EnemyState.Alert;
                break;
            case EnemyState.Alert:
                DoEngageDelay();
                RotateCanonToPlayer();
                break;
            case EnemyState.Chase:
                ChaseAfterPlayer();
                RotateCanonToPlayer();
                break;
            
            case EnemyState.Investigate:
                LookForPlayer();
                
                break;
            
            case EnemyState.Attack:
                RotateCanonToPlayer();
                CheckIfCanAttack();
                enemyAttackScript.Attack();
                break;
            
            default:
                currentState = EnemyState.Patrol;
                break;
        }
        text.text = currentState.ToString();

        if (IsPlayerInLineOfSight() && currentState != EnemyState.Investigate)
        {
            lastPlayerPosition = player.transform.position;
        }

        if (currentState == EnemyState.Patrol)
        {
            spotLight.color = normalColor;
        }
        else if (currentState == EnemyState.Alert || currentState== EnemyState.Investigate)
        {
            spotLight.color = alertedColor;
        }
        else if(currentState == EnemyState.Chase || currentState == EnemyState.Attack)
        {
            spotLight.color = detectedColor;
        }
        
        
    }


    #region Patrol Logic
    void SelectAndMoveToNewPoint()
    {
        if (!hasSelectedNextPoint)
        {
            Vector3 nextPoint = new Vector3(transform.position.x + Random.Range(-maxNextPointDistance,maxNextPointDistance), 0, transform.position.z + Random.Range(-maxNextPointDistance,maxNextPointDistance));
            agent.SetDestination(nextPoint);
            
            if (!agent.hasPath || agent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
               return;
            }
            
            hasSelectedNextPoint = true;
            Debug.Log("Selected Next Point");
            
        }
        WaitOnPatrolPoint();
    }

    void WaitOnPatrolPoint()
    {
        if (agent.velocity.magnitude < 0.1f || (agent.destination-transform.position).magnitude < 0.5f)
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
    
    #endregion
    
    #region Alert Logic

    void DoEngageDelay()
    {
        
        if (CanSeePlayerInCone())
        {
            currentEngageDelayTime += Time.deltaTime;
            if (currentEngageDelayTime >= engageDelayTime)
            {
                currentEngageDelayTime = 0;
                currentState = EnemyState.Chase;
            }
        }
        else
        {
            currentEngageDelayTime = 0;
            currentState = EnemyState.Patrol;
        }
    }
    
    
    
    #endregion

    #region Chase Logic

    void ChaseAfterPlayer()
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer > distanceToAttackPlayer && IsPlayerInLineOfSight())
        {
            agent.SetDestination(player.transform.position);
        }
        else if (!IsPlayerInLineOfSight())
        {
            investigationTime = Random.Range(minInvestigationTime, maxInvestigationTime);
            currentState = EnemyState.Investigate;
            return;
        }

        if (distanceToPlayer <= distanceToAttackPlayer && IsPlayerInLineOfSight())
        {
            currentState = EnemyState.Attack;
            agent.SetDestination(transform.position);
        }
    }

    #endregion

    #region Investigate Logic

    void LookForPlayer()
    {
        agent.SetDestination(lastPlayerPosition);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(lastPlayerPosition, out hit, 3f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
        else
        {
            // Fallback to patrol if the position is invalid
            Debug.LogWarning("lastPlayerPosition was off NavMesh. Returning to Patrol.");
            currentState = EnemyState.Patrol;
            return;
        }

        if (CanSeePlayerInCone()) currentState = EnemyState.Chase;
        
        if (agent.velocity.magnitude < 0.1f|| (agent.destination-transform.position).magnitude < 0.5f)
        {
            currentInvestigationTime += Time.deltaTime;
            RotateCanon();
            if (CanSeePlayerInCone())
            {
                currentState = EnemyState.Chase;
                currentInvestigationTime = 0;
                return;
            }

            if (currentInvestigationTime >= investigationTime && !CanSeePlayerInCone())
            {
                currentState = EnemyState.Patrol;
                currentInvestigationTime = 0;
                return;
            }
            
        }
        else
        {
            RotateCanon();
        }
    }

    #endregion

    #region Attack Logic

    void CheckIfCanAttack()
    {
        if (Vector3.Distance(player.transform.position, transform.position) >= distanceToBreakAttack && CanSeePlayerInCone())
        {
            currentState = EnemyState.Chase;
        }
        else if(!IsPlayerInLineOfSight() )
        {
            
            currentState = EnemyState.Investigate;
        }
        
        
    }

    #endregion
    
    #region Global Functions

    void RotateCanonToPlayer()
    {
        
        Vector3 targetPosition = player.transform.position;
        
        Vector3 direction = (targetPosition - turretTransform.position).normalized; 
        
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float currentAngle = turretTransform.eulerAngles.y;
        
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turretProperties.rotateSpeed * Time.deltaTime);
        turretTransform.rotation = Quaternion.Euler(0,newAngle,0);
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
    
    #endregion
    
    #region Player Detection Logic
    //Player Detection
    bool CanSeePlayerInCone()
    {
        float halfFieldOfView = viewAngle / 2;
        float angleIncrement = viewAngle / rayCount;
        float angleOffset = turretTransform.forward.y - halfFieldOfView;
        for (int i = 0; i < rayCount; i++)
        {
            Vector3 rayDirection = Quaternion.Euler(0, angleOffset, 0) * turretTransform.forward;
            
            Vector3 rayOrigin = new Vector3(turretTransform.position.x, turretTransform.position.y, turretTransform.position.z + 0.5f);

            if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, viewDistance))
            {
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);
                    return true;
                }
                else
                {
                    Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.yellow);
                    
                }
            }
            else
            {
                Debug.DrawRay(rayOrigin, rayDirection * viewDistance, Color.red);
            }
            angleOffset += angleIncrement;
        }
        return false;
    }

    bool IsPlayerInLineOfSight()
    {
        Vector3 directionToPlayer = player.transform.position - turretTransform.position;
        directionToPlayer = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
        float angleToPlayer = Mathf.Atan2(directionToPlayer.z,directionToPlayer.x) * Mathf.Rad2Deg;
        
        Vector3 rayOrigin = new Vector3(turretTransform.position.x, player.transform.position.y, turretTransform.position.z + 0.5f);
        Vector3 rayDirection = directionToPlayer.normalized;

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.collider.CompareTag("Player"))
            {
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.green);
                return true;
            }
            else
            {
                Debug.DrawRay(rayOrigin, rayDirection * hit.distance, Color.red);
            }
        }
        return false;
    }
    #endregion
    
    
}
