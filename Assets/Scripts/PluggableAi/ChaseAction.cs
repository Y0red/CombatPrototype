using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/action/chase action")]
public class ChaseAction : AbstractAction
{
    public override void Act(StateController controller)
    {
        Chase(controller);
    }

    private void Chase(StateController controller)
    {
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }
}
