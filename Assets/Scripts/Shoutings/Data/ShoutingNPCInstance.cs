using System;
using UnityEngine;

public enum ShoutingPosition
{
    Position1 = 0,
    Position2 = 1,
    Position3 = 2,
    Position4 = 3,
    Position5 = 4
}

[Serializable]
public class ShoutingNPCInstance
{
    public ShoutingNPC npc;

    public ShoutingPosition position;
}