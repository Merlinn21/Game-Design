using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Ghost", menuName ="Ghost/Create new Ghost")]
public class GhostBase : ScriptableObject
{
    [SerializeField] private string ghostName;
    [SerializeField] private Sprite sprite;

    [SerializeField] private int maxHP;
    [SerializeField] private int maxMana;
    [SerializeField] private int atk;
    [SerializeField] private int def;

    [SerializeField] private List<Move> moveList;

    public string getName() { return ghostName; }
    public Sprite getSprite() { return sprite; }
    public int getMaxHp() { return maxHP; }
    public int getMaxMana() { return maxMana; }
    public int getAtk() { return atk; }
    public int getDef() { return def; }
    public List<Move> getMoveList() { return moveList; }
}
[System.Serializable]
public class Move
{
    [SerializeField] GhostMoveBase moveBase;

    public GhostMoveBase getMoveBase() { return moveBase; }
}
