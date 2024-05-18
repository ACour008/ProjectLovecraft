using UnityEngine;
public class Enemy : WorldActor
{
    EnemyBehaviour enemyBehaviour;

    void Start()
    {
        enemyBehaviour = new EnemyBehaviour(this);
    }

    void Update()
    {
        enemyBehaviour.OnUpdate();
    }

    void OnDrawGizmosSelected()
    {
        GizmoCircleDrawer.Draw(transform.position, enemyBehaviour.chaseRange, Color.green);
        GizmoCircleDrawer.Draw(transform.position, enemyBehaviour.attackRange, Color.red);
    }
}
