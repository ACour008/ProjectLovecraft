using UnityEngine;
using UnityEngine.AI;

public class IdleState : EnemyState
{
    public IdleState(EnemyBehaviour behaviour) : base(behaviour) {}

    public override void Update()
    {
        var distanceToTarget = Vector3.Distance(behaviour.actor.transform.position, behaviour.target.position);
        if (distanceToTarget <= behaviour.chaseRange)
        {
            behaviour.state.ChangeState(new ChaseState(behaviour));
        }
    }
}