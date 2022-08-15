using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/look")]
public class LookDecisions : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        RaycastHit hit;

        Debug.DrawRay(controller.eyes.position, controller.eyes.forward.normalized * controller.aggroRange, Color.green);

        if(Physics.SphereCast(controller.eyes.position, controller.aggroRange, controller.eyes.forward, out hit, controller.aggroRange) && hit.collider.CompareTag("Player"))
        {
            controller.chaseTarget = hit.transform;
            return true;
        }
        else
        {
            return false;
        }
    }
}
