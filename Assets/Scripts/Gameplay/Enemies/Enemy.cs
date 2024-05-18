using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : WorldActor
{
    CharacterProfile profile;

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
    }

}
