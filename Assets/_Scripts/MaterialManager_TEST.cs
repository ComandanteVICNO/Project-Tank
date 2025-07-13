// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using System.Collections.Generic;
using UnityEngine;

public class MaterialManager_TEST : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerObjects;
    
    [SerializeField] private List<Material> playerMaterials;


    private void Update()
    {
        if (InputManager.instance.inputActions.Player.Color1.WasPerformedThisFrame())
        {
            Debug.Log("Color1 was performed");
            foreach (GameObject playerObject in playerObjects)
            {
                Renderer playerRenderer = playerObject.GetComponent<Renderer>();
                playerRenderer.material = playerMaterials[0];
            }
        }
        if (InputManager.instance.inputActions.Player.Color2.WasPerformedThisFrame())
        {
            Debug.Log("Color2 was performed");
            foreach (GameObject playerObject in playerObjects)
            {
                Renderer playerRenderer = playerObject.GetComponent<Renderer>();
                playerRenderer.material = playerMaterials[1];
            }
        }
        if (InputManager.instance.inputActions.Player.Color3.WasPerformedThisFrame())
        {
            Debug.Log("Color3 was performed");
            foreach (GameObject playerObject in playerObjects)
            {
                Renderer playerRenderer = playerObject.GetComponent<Renderer>();
                playerRenderer.material = playerMaterials[2];
            }
        }
    }
}
