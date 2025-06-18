// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public InputSystem_Actions inputActions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        inputActions = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return inputActions.Player.Move.ReadValue<Vector2>();
    }
}
