using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDebug : MonoBehaviour, IAttackable
{
    public void OnAttack(GameObject attacker, Attack attack)
    {
        if (attack.IsCritical)
            Debug.Log("CRITICAL DAMAGE");

        Debug.LogFormat("{0} attacke{1} for {2} damage.", attacker.name, name, attack.Damage);
    }
}
