// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TankTorretRotation : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform turretTransform;
    [SerializeField] private GunProperties turrerProperties;
    
    [SerializeField] private LayerMask groundLayer;

    private void Awake()
    {
        playerCamera = Camera.main;
    }
    
    private void Update()
    {
        RotateCannonToMouse();
    }

    private void RotateCannonToMouse()
    {
        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();

        Ray ray = playerCamera.ScreenPointToRay(mouseScreenPosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - turretTransform.position).normalized; 
            
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float currentAngle = turretTransform.eulerAngles.y;
        
            float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turrerProperties.rotateSpeed * Time.deltaTime);
            turretTransform.rotation = Quaternion.Euler(0,newAngle,0);
        }
    }
}
