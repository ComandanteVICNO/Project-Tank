// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.
using UnityEngine;

[CreateAssetMenu(fileName = "NewTankGunData", menuName = "Tank/GunData")]
public class GunProperties : ScriptableObject
{
    public float damage;
    public float fireRate;
    public float range;
    public float rotateSpeed;
    public bool isFiring;
}
