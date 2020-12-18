using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerStat :MonoBehaviour
{
    static int baseAtk = 260;
    static int baseSAtk = 260;
    static int baseSDef = 210;
    static int baseDef = 210;
    static int baseHealth = 450;
    static int baseMana = 450;
    static public int lvl = 5;

    static public int maxHealth = (((((baseHealth + 10) * 2) + 1) * lvl) / 100) + lvl + 10;
    static public int health = maxHealth;
    static public int maxMana = (((((baseMana + 10) * 2) + 1) * lvl) / 100) + lvl + 10;
    static public int mana = maxMana;
    static public int atk = (((((baseAtk + 10) * 2) + 1) * lvl) / 100) + 5;
    static public int satk = (((((baseSAtk + 10) * 2) + 1) * lvl) / 100) + 5;
    static public int def = (((((baseDef + 10) * 2) + 1) * lvl) / 100) + 5;
    static public int sdef = (((((baseSDef + 10) * 2) + 1) * lvl) / 100) + 5;

    static public int move1 = 0;
    static public int move2 = 1;
    static public int move3 = 2;
    static public int move4 = 3;

    
    static public int exp = 0;
    static public int maxExp = 100;
}

