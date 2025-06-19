// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool hasProjectileBeenInitialized = false;
    
    Vector3 projectileDirection;
    float projectileSpeed;
    private int projectileDamage;
    float projectileLifeTime;
    
    
    public virtual void InitializeProjectile(Vector3 direction, float speed, int damage, float lifeTime)
    {
        projectileDirection = direction;
        projectileSpeed = speed;
        projectileDamage = damage;
        projectileLifeTime = lifeTime;
        hasProjectileBeenInitialized = true;
    }

    private void Update()
    {
        if (!hasProjectileBeenInitialized) return;
        MoveProjectile();
        DecayProjectile();
    }

    public virtual void MoveProjectile() //Default projectile movement, might add different types  in the future
    {
        transform.Translate(projectileDirection * projectileSpeed * Time.deltaTime, Space.World);
    }

    public virtual void DecayProjectile()
    {
        projectileLifeTime -= Time.deltaTime;
        if (projectileLifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(projectileDamage);
        }
        Destroy(gameObject);
    }
}
