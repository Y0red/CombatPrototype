using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackedScrolText : MonoBehaviour, IAttackable
{
    public ScrolingTExt text;
    public Color textColor;
    public void OnAttack(GameObject attacker, Attack attack)
    {
        var textt = attack.Damage.ToString();

        var scrolingText = Instantiate(text, transform.position, Quaternion.identity);
        scrolingText.SetText(textt);
        scrolingText.SetColor(textColor);
    }
}
