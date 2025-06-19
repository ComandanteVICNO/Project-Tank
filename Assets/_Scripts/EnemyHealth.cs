// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 5;
    [SerializeField] private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (enemyAI.currentState == EnemyAI.EnemyState.Patrol || enemyAI.currentState == EnemyAI.EnemyState.Alert ||
            enemyAI.currentState == EnemyAI.EnemyState.Alert)
        {
            enemyAI.currentState = EnemyAI.EnemyState.Chase;
        }
    }

    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
