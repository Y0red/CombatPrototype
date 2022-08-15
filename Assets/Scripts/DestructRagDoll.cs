using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructRagDoll : MonoBehaviour, IDestructable
{
    public RagDoll RagDollObject;
    public float Force;
    public float Lift;

    public event Action IDied;

    public void OnDestruction(GameObject destroyer)
    {
        var ragdoll = Instantiate(RagDollObject, transform.position, transform.rotation);

        var vectorFromDestroyer = transform.position - destroyer.transform.position;
        vectorFromDestroyer.Normalize();
        vectorFromDestroyer.y += Lift;

        ragdoll.ApplyForce(vectorFromDestroyer * Force);
    }
}
