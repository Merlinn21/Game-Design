using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Ghost", menuName ="Ghost/Create new Ghost")]
public class GhostBase : ScriptableObject
{
    [SerializeField] private string ghostName;
    [SerializeField] private Sprite sprite;

    [SerializeField] private float maxHP;
    [SerializeField] private float atk;
    [SerializeField] private float def;
    [SerializeField] private float speed;

    public string getName() { return ghostName; }
    public Sprite getSprite() { return sprite; }
    public float getMaxHp() { return maxHP; }
    public float getAtk() { return atk; }
    public float getDef() { return def; }
    public float getSpeed() { return speed; }
}
