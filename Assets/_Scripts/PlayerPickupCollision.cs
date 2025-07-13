// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class PlayerPickupCollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        IPickupable pickupable = other.GetComponent<IPickupable>();
        if (pickupable != null)
        {
            pickupable.Pickup();
        }
    }
}
