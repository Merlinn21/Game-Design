using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost
{
    public GhostBase Base { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }

    public List<GhostMove> moveList { get; set; }

    public Ghost(GhostBase ghostBase, int level)
    {
        Base = ghostBase;
        Level = level;
        HP = Base.getMaxHp();

        moveList = new List<GhostMove>();
        foreach (var move in Base.getMoveList())
        {
            moveList.Add(new GhostMove(move.getMoveBase()));
        }
    }

    public int MaxHp()
    {
        return Mathf.FloorToInt(Base.getMaxHp() * Level / 5);
    }

    public int Attack()
    {
        return Mathf.FloorToInt(Base.getAtk());
    }

    public int Defend()
    {
        return Mathf.FloorToInt(Base.getDef());
    }

    public int MaxMana()
    {
        return Mathf.FloorToInt(Base.getMaxMana() * Level / 5);
    }

}
