// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int health = 5;
    private int maxhealth;

    private void Awake()
    {
        maxhealth = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    private void Update()
    {
        UIManager.instance.UpdateHealthText(health, maxhealth);
        
        if (health <= 0)
        {
            GameManager.instance.EndGame();
            Destroy(gameObject);
        }
    }
}
