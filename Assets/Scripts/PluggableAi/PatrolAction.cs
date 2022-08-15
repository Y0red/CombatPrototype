using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/Actions/Patrol")]
public class PatrolAction : AbstractAction
{
    public override void Act(StateController controller)
    {
        Patrol(controller);
    }

    private void Patrol(StateController controller)
    {
        controller.navMeshAgent.destination = controller.waypointsAll[controller.nextWaypoint].position;
        controller.navMeshAgent.isStopped = false;

        if(controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
        {
            controller.nextWaypoint = (controller.nextWaypoint + 1) % controller.waypointsAll.Count;
        }
    }
}
