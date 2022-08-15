using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "pluggableAI/decisions/Scan")]
public class ScanDecision : Decesion
{
    public override bool Decide(StateController controller)
    {
        bool noEnemyInSight = Scan(controller);
        return noEnemyInSight;
    }

    private bool Scan(StateController controller)
    {
        controller.navMeshAgent.isStopped  = true;
        controller.transform.Rotate(0, .2f * Time.deltaTime, 0);
        return controller.CheckIfCountDownElapsed(.2f);
    }
}
