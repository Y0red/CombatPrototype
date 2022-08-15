using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack.asset", menuName = "Attack/BaseAttack")]
public class AttackDifination : ScriptableObject
{
    public float Cooldown;
    public float Range;
    public float minDamage;
    public float maxDamage;
    public float criticalMultiplyer;
    public float criticalChance;

    public Attack CreatAttack(CharacterStats wilderStats, CharacterStats defenderStats)
    {
        float coreDamage = wilderStats.GetDamage();
        coreDamage += Random.Range(minDamage, maxDamage);

        bool isCritical = Random.value < criticalChance;
        if (isCritical)
            coreDamage += criticalMultiplyer;

        if(defenderStats != null)
            coreDamage -= defenderStats.GetResistance();


        return new Attack((int)coreDamage, isCritical);

    }
}
