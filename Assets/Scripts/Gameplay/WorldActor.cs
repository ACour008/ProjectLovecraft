using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class WorldActor : MonoBehaviour
{
    [SerializeField] LineRenderer _weaponLineRenderer;

    public LineRenderer weaponLineRenderer
    {
        get => _weaponLineRenderer;
        set => _weaponLineRenderer = value;
    }
    public virtual void TakeDamage(int damage)
    {
        
    }
}
