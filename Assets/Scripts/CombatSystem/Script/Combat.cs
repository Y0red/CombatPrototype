using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat.asset", menuName = "Attack/Combat")]
public class Combat : AttackDifination
{
    public Rigidbody weaponPrifab;

    public void ExecuteAttack(GameObject attacker, GameObject defender)
    {
        if (defender == null)
            return;

        //check if defender is in range of the attacker
        if (Vector3.Distance(attacker.transform.position, defender.transform.position) > Range)
            return;

        //check if defender is in front of the player
         if (!attacker.transform.IsFacingTarget(defender.transform))
             return;

        //at this point we create an attack
        var attackerStats = attacker.GetComponent<CharacterStats>();
        var defenderStats = defender.GetComponent<CharacterStats>();

        var attack = CreatAttack(attackerStats, defenderStats);

        var attackables = defender.GetComponentsInChildren(typeof(IAttackable));

        foreach (IAttackable a in attackables)
        {
            a.OnAttack(attacker, attack);
        }
    }
}
