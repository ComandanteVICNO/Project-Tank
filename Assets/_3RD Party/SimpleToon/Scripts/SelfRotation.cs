using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelfRotation : MonoBehaviour
{
    public float Speed;
    public Vector3 RotationVector;

    public void Update()
    {
        transform.Rotate(RotationVector, Speed * Time.deltaTime);
    }
}
