using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Line
{
    public Character character;

    [TextArea(2, 5)]
    public string text;
    
}

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/Create new Dialogue")]
public class Dialogue : ScriptableObject
{
    [SerializeField] private Character charLeft;
    [SerializeField] private Character charRight;
    public Line[] lines;
    public GhostParty ghostParty;
    [SerializeField] private bool freeRoam;

    public Character getCharLeft() { return charLeft; }
    public Character getCharRight() { return charRight; }
    public bool getFreeRoam() { return freeRoam; }
}
