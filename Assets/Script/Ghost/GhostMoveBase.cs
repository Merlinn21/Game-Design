using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum targetType
{
    Friendly,
    Enemy,
    Self,
    Aoe
}

public enum costType
{
    Hp,
    Mana
}

public enum moveType
{
    Fire,
    Water,
    Earth,
    Wind,
    Dark,
    Light,
    Physical
}

[CreateAssetMenu(fileName = "Ghost", menuName = "Ghost/Create new Move")]
public class GhostMoveBase : ScriptableObject
{
    [SerializeField] private string moveName;
    [SerializeField] private float baseDmg;
    [SerializeField] private float acc;
    [SerializeField] private float cost;
    [SerializeField] private costType costType;
    [SerializeField] private targetType targetType;
    [SerializeField] private moveType moveType;

    public string getMoveName() { return moveName; }
    public float getBaseDmg() { return baseDmg; }
    public float getAcc() { return acc; }
    public float getCost() { return cost; }
    public costType getCostType() { return costType; }
    public targetType getTargetType() { return targetType; }
    public moveType GetMoveType() { return moveType; }
}
