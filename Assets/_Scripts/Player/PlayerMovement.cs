// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody rb;
    
    [Header("Movement")]
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotatinSpeed;
    
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();    
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    void HandleMovement()
    {
        Vector2 playerInputs = InputManager.instance.GetPlayerMovement();
        
        float xMovement = playerInputs.x;
        float yMovement = playerInputs.y;

        //Check if player is moving
        if (playerInputs.magnitude > 0.1f && InputManager.instance.inputActions.Player.Move.IsPressed())
        {
            //Movement force
            rb.AddForce((Vector3.forward * yMovement * acceleration) + (Vector3.right * xMovement * acceleration));
            
            //Rotation
            Vector3 moveDirection = new Vector3(xMovement, 0, yMovement).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,rotatinSpeed * Time.fixedDeltaTime);
        }
        else if(playerInputs.magnitude > 0.001f)
        {
            Vector3 oppositeDirection = -rb.linearVelocity.normalized * deceleration;
            rb.AddForce(oppositeDirection);
        }
        
        //Clamp max speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            Vector3 clampedVelocity = rb.linearVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(clampedVelocity.x, rb.linearVelocity.y, clampedVelocity.z);
        }
        
    }
}
