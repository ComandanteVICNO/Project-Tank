// © 2025 Francisco Gonçalves. All Rights Reserved.
// For portfolio viewing only – usage or redistribution is prohibited.

using System;
using UnityEngine;

public class MainMenuTankSpin : MonoBehaviour
{
    [SerializeField] Transform tankTransform;
    [SerializeField] private float spinSpeed;


    private void Update()
    {
        float newY = tankTransform.eulerAngles.y - spinSpeed * Time.deltaTime;
        tankTransform.rotation = Quaternion.Euler(0f, newY, 0f);
    }
}
