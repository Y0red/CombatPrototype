using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructedRaiseEvent : MonoBehaviour, IAttackable
{
    private NPCController mob;
    private void Awake()
    {
        mob = GetComponent<NPCController>();
    }
    public void OnAttack(GameObject attacker, Attack attack)
    {
        mob.OnMobDeath.Invoke(mob.mobType, transform.position);
    }
}
