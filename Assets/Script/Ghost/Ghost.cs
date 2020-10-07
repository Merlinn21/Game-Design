using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost
{
    public GhostBase Base { get; set; }
    public int Level { get; set; }

    public Ghost(GhostBase ghostBase, int level)
    {
        Base = ghostBase;
        Level = level;
    }

    public int MaxHp()
    {
        return Mathf.FloorToInt(Base.getMaxHp());
    }

    public int Attack()
    {
        return Mathf.FloorToInt(Base.getAtk());
    }

    public int Defend()
    {
        return Mathf.FloorToInt(Base.getDef());
    }

    public int Speed()
    {
        return Mathf.FloorToInt(Base.getSpeed());
    }
}
