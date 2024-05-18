using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour
{
    public EnemyBehaviourState state;
    public WorldActor actor;
    public Transform target => Shell.instance.player.transform;
    public float chaseRange;
    public float attackRange;
    public List<Attack> attacks = new List<Attack>();

    public EnemyBehaviour(WorldActor actor)
    {
        this.actor = actor;
        SetStats(actor.profile);

        state = new EnemyBehaviourState();
        state.ChangeState(new IdleState(this));
    }

    void SetStats(CharacterProfile profile)
    {
        chaseRange = profile.chaseRange;
        attackRange = profile.attackRange;
        attacks = profile.attacks;
    }

    public void OnUpdate()
    {
        state.Update();
    }
}

public abstract class EnemyState : IState
{
    protected EnemyBehaviour behaviour;

    public EnemyState(EnemyBehaviour behaviour)
    {
        this.behaviour = behaviour;
    }

    public virtual void Enter(){  }

    public virtual void Exit() {  }

    public virtual void Update() {  }
}