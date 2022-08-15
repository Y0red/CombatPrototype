using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawn 
{
   Rigidbody itemSpawned { get; set; }

    Renderer itemMaterial { get; set; }

    ItemPickUp itmeType { get; set; }

    void CreateSpawn();
}
