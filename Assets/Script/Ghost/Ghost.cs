using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost
{
    public GhostBase Base { get; set; }
    public int Level { get; set; }
    public int HP { get; set; }
    public bool alive = true;

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

    public int MaxMana()
    {
        return Mathf.FloorToInt(Base.getMaxMana());
    }

    public int TakeDmg(GhostMoveBase move)
    {
        int dmg = (int)move.getBaseDmg();
        HP -= dmg;
        if(HP <= 0)
        {
            HP = 0;
        }
        return dmg;
    }

    public GhostMove getRandomMove()
    {
        int random = Random.Range(0, moveList.Count);
        return moveList[random];
    }

    public int GiveDmg(GhostMoveBase move)
    {
        int dmg = (int)move.getBaseDmg();
        PlayerStat.health -= dmg;


        return dmg;
    }
}
