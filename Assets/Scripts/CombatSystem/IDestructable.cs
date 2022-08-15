using System;
using UnityEngine;

public interface IDestructable
{
    event Action IDied;

    void OnDestruction(GameObject destroyer);
}
