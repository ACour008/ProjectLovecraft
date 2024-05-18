using UnityEngine;

[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Create/Character/Combat/MeleeAttack")]
public class MeleeAttack : Attack
{
    public override bool CanExecute()
    {
        return true;
    }

    public override void Execute()
    {
        Debug.Log("Executing Melee Attack");
    }
}