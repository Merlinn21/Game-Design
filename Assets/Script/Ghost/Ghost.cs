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
    public Dictionary<Stat, int> Stats { get; private set; }
    public Dictionary<Stat, int> StatBoost { get; private set; }

    public Ghost(GhostBase ghostBase, int level)
    {
        Base = ghostBase;
        Level = level;
        

        moveList = new List<GhostMove>();
        foreach (var move in Base.getMoveList())
        {
            moveList.Add(new GhostMove(move.getMoveBase()));
        }
        CalculateStats();
        HP = MaxHp;

        StatBoost = new Dictionary<Stat, int>()
        {
            {Stat.Attack,0 },
            {Stat.SAttack,0 },
            {Stat.Defense,0 },
            {Stat.SDefense,0 }
        };
    }

    private void CalculateStats()
    {
        Stats = new Dictionary<Stat, int>();
        Stats.Add(Stat.Attack, (((((Base.getAtk() + 10) * 2) + 1) * Level) / 100) + 5);
        Stats.Add(Stat.SAttack, (((((Base.getSAtk() + 10) * 2) + 1) * Level) / 100) + 5);
        Stats.Add(Stat.Defense, (((((Base.getDef() + 10) * 2) + 1) * Level) / 100) + 5);
        Stats.Add(Stat.SDefense, (((((Base.getSDef() + 10) * 2) + 1) * Level) / 100) + 5);

        MaxHp = (((((Base.getMaxHp() + 10) * 2) + 1) * Level) / 100) + Level + 10;
    }

    int GetStats(Stat stat)
    {
        int statVal = Stats[stat];

        int boost = StatBoost[stat];
        var boostValues = new float[] { 1f, 1.5f, 2f, 2.5f, 3f, 3.5f, 4f };

        if (boost >= 0)
        {
            statVal = Mathf.FloorToInt(statVal * boostValues[boost]);
        }
        else
        {
            statVal = Mathf.FloorToInt(statVal / boostValues[boost]);
        }

        return statVal;
    }

    //-----------------------GhostStats----------------------------
    public int MaxHp
    {
        get; private set;
    }

    public int Atk {
        get{ return Stats[Stat.Attack]; }
    }

    public int SAtk
    {
        get { return Stats[Stat.SAttack]; }
    }

    public int Def
    {
        get { return Stats[Stat.Defense]; }
    }

    public int SDef
    {
        get { return Stats[Stat.SDefense]; }
    }


    //-----------------------GhostMove----------------------------
    public int TakeDmg(GhostMoveBase move)
    {
        var mod = Random.Range(0.75f, 1.1f);
        var dmg = 0f;
        if (move.GetMoveType() == moveType.Physical)
        {
            dmg = (((((2 * PlayerStat.lvl/5) + 2) * move.getBaseDmg() * PlayerStat.atk / Def)/ 50) + 2)  * mod;
        }
        else
        {
            dmg = (((((2 * PlayerStat.lvl / 5) + 2) * move.getBaseDmg() * PlayerStat.satk / SDef) / 50) + 2) * mod;
        }

        HP -= Mathf.RoundToInt(dmg);

        if(HP <= 0)
        {
            HP = 0;
        }

        return Mathf.RoundToInt(dmg);
    }

    public GhostMove getRandomMove()
    {
        int random = Random.Range(0, moveList.Count);

        return moveList[random];
    }

    public int GiveDmg(GhostMoveBase move)
    {
        var mod = Random.Range(0.75f, 1.1f);
        var dmg = 0f;

        if (move.GetMoveType() == moveType.Physical)
        {
            dmg = (((((2 * Level / 5) + 2) * move.getBaseDmg() * Atk / PlayerStat.def) / 50) + 2) * mod;
        }
        else
        {
            dmg = (((((2 * Level / 5) + 2) * move.getBaseDmg() * SAtk / PlayerStat.sdef) / 50) + 2) * mod;
        }
        PlayerStat.health -= Mathf.RoundToInt(dmg);

        return Mathf.RoundToInt(dmg);
    }

    public int GiveHeal(GhostMoveBase move)
    {
        var heal = (move.getBaseDmg() * MaxHp)/100;

        HP += (int)heal;

        if(HP >= MaxHp)
        {
            HP = MaxHp;
        }

        return (int)heal;
    }
}
