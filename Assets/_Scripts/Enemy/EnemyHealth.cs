// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 4;
    [SerializeField] private int damagedHealth;
    [SerializeField] private EnemyAI enemyAI;
    [SerializeField] private int scoreWhenKilled;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private GameObject destroyParticles;
    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();
        damageParticles.gameObject.SetActive(false);
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
        if (health <= damagedHealth)
        {
            damageParticles.gameObject.SetActive(true); 
            damageParticles.Play();
        }
        if (health <= 0)
        {
            GameManager.instance.AddScore(scoreWhenKilled);
            
            
            GameObject deathParticles = Instantiate(destroyParticles, transform.position, Quaternion.identity);
            ParticleSystem ps = deathParticles.GetComponent<ParticleSystem>();
            ps.Play();
            Destroy(deathParticles, 5f);
            
            
            Destroy(gameObject);
        }
    }
}
