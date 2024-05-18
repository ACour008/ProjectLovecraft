using Unity.Burst.Intrinsics;
using UnityEngine;

public class AttackState : EnemyState
{
    private float _attackCooldown;
    public AttackState(EnemyBehaviour behaviour) : base(behaviour) {}


    public override void Enter()
    {
        _attackCooldown = 0;
    }

    public override void Update()
    {
        var distanceToTarget = Vector3.Distance(behaviour.actor.transform.position, behaviour.target.position);
        if (distanceToTarget > behaviour.attackRange)
        {
            behaviour.state.ChangeState(new ChaseState(behaviour));
            return;
        }

        if (_attackCooldown <= 0f)
        {
            Attack();
            _attackCooldown = 2f;
        }

        _attackCooldown -= Time.deltaTime;
    }

    void Attack()
    {
        foreach (var attack in behaviour.attacks)
        {
            if (attack.CanExecute())
            {
                attack.Execute();
                break;
            }
        }
    }
}
