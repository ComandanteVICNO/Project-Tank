// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class CoinPickupLogic : MonoBehaviour,IPickupable
{
    [SerializeField] private int pickupScoreAmmount;
    [SerializeField] private GameObject coinPickupVisualObject;
    [SerializeField] private float timeUntilRespawn;
    float elapsedTime;
    bool wasPickedUp = false;
    
    
    
    public void Pickup()
    {
        if (wasPickedUp) return;
        wasPickedUp = true;
        GameManager.instance.AddScore(pickupScoreAmmount);
        coinPickupVisualObject.SetActive(false);
    }


    private void Update()
    {
        if (wasPickedUp)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeUntilRespawn)
            {
                elapsedTime = 0;
                coinPickupVisualObject.SetActive(true);
                wasPickedUp = false;
            }
        }
    }
}
