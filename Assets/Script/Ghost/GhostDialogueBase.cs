using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ghost", menuName = "Ghost/Create new Ghost Dialogue")]
public class GhostDialogueBase : ScriptableObject
{
    public GhostLine[] ghostLines;
    public int bonus;
    public BonusStat bonusType;
}

[System.Serializable]
public class GhostLine
{
    public int textId;
    public int nextText;
    [TextArea(2, 5)]
    public string text;
    public MultiChoice[] multiChoices;
    public bool end;
    public bool success;
    
}

[System.Serializable]
public class MultiChoice
{
    public int nextText;
    public string choiceText;
    [TextArea(2, 5)]
    public string responseText;
}

public enum BonusStat
{
    Attack,
    SAttack,
    Defense,
    SDefense,
    HP,
    MP
}
