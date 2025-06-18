// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class PlayerTurretAttack : MonoBehaviour
{
    [SerializeField] private GunProperties turretProperties;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform attackPoint;
    private float timeToAttack = 0;
    
    private void Update()
    {
        timeToAttack -= Time.deltaTime;
        if (InputManager.instance.inputActions.Player.Attack.IsPressed())
        {
            if (timeToAttack <= 0)
            {
                turretProperties.isFiring = true;
                
                GameObject projectile = Instantiate(projectilePrefab,attackPoint.position, attackPoint.rotation);
                Projectile projectileScript = projectile.GetComponent<Projectile>();
                projectileScript.InitializeProjectile(attackPoint.forward,turretProperties.projectileSpeed,turretProperties.damage, turretProperties.projectileLifetime);
                timeToAttack = turretProperties.fireRate;
                
                turretProperties.isFiring = false;
            }
        }    
    }
}
