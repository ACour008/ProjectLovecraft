using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[Flags]
public enum Direction
{
    None = 0,
    North = 0b1000,
    East = 0b0100,
    South = 0b0010,
    West = 0b0001
}

public class Room : MonoBehaviour
{
    public Direction doorOpenings;

}
