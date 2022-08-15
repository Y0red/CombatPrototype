using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class AttackedTakeDamage : MonoBehaviour, IAttackable
{
    private CharacterStats stats;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }
    public void OnAttack(GameObject attacker, Attack attack)
    {
        stats.GetComponent<Animator>().SetTrigger("Hit");
        stats.TakeDamage(attack.Damage);
        if(stats.GetHealth() <= 0)
        {
            //destroy object
            var destructables = GetComponents(typeof(IDestructable));
            foreach(IDestructable d in destructables)
            {
                d.OnDestruction(attacker);
            }
        }
    }
}
