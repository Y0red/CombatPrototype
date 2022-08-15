using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDoll : MonoBehaviour
{
    public Rigidbody RagdollCore;
    public float timeToLive;
    void Start()
    {
        Destroy(gameObject, timeToLive);
    }

    public void ApplyForce(Vector3 force)
    {
        RagdollCore.AddForce(force);
    }

    
}
