using System.Collections;
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
    public WorldActorType actorType;

    public GameObject prefab;
    public string title;
}
