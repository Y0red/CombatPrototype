using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/Attack")]
public class AttackAction : AbstractAction
{
    public override void Act(StateController controller)
    {
        Attackk(controller);
    }

    private void Attackk(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.attack.Range, Color.blue);
        if (Physics.SphereCast(controller.eyes.position, controller.aggroRange, controller.eyes.forward, out hit, controller.attack.Range) && hit.collider.CompareTag("Player"))
        {
            Debug.Log("Attacking");
            if (controller.CheckIfCountDownElapsed(controller.attack.Cooldown))
            {
                //fire
                Debug.Log("Attacking");
            }
        }
    }
}
