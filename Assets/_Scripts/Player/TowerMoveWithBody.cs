// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class TowerMoveWithBody : MonoBehaviour
{
    [SerializeField] private Transform targetLocation;

    private void LateUpdate()
    {
        transform.position = targetLocation.position;
    }
}
