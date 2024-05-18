using UnityEngine;

[CreateAssetMenu(fileName = "RangedAttack", menuName = "Create/Character/Combat/RangeAttack")]
public class RangeAttack : Attack
{
    public override bool CanExecute()
    {
        return true;
    }

    public override void Execute()
    {
        Debug.Log("Executing Range Attack");
    }
}