using System.Collections.Generic;
using UnityEngine;

public enum WorldActorType
{
    Enemy,
    Boss,
    Neutral,
    Player
}

[CreateAssetMenu(fileName = "CharacterProfile", menuName = "Create/Character/CharacterProfile")]
public class CharacterProfile : ScriptableObject
{
    public bool isPlayer;

    [Header("Basic info")]
    public WorldActorType actorType;
    public GameObject prefab;
    public string title;

    [Header("AI/Combat")]
    public int maxHealth;
    public int maxDamage;
    public float chaseRange = 8f;
    public float attackRange = 2f;
    public List<Attack> attacks;
}
