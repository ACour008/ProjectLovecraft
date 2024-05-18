using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyBehaviour behaviour) : base(behaviour) { }

    public override void Update()
    {
        if (!behaviour.target)
            return;

        var distanceToTarget = Vector3.Distance(behaviour.actor.transform.position, behaviour.target.position);
        if (distanceToTarget <= behaviour.attackRange)
        {
            behaviour.state.ChangeState(new AttackState(behaviour));
            return;
        }

        if (distanceToTarget >= behaviour.chaseRange)
        {
            behaviour.state.ChangeState(new IdleState(behaviour));
        }

        Chase();
    }

    void Chase()
    {
        behaviour.actor.transform.position = Vector3.MoveTowards(behaviour.actor.transform.position, behaviour.target.position, Time.deltaTime * 1f);
    }
}