﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedEvent : MonoBehaviour,IDestructable
{
    public event Action IDied;
    public void OnDestruction(GameObject destroyer)
    {
        if(IDied != null)
        {
           IDied();
        }
    }

}
