using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMove
{
    public GhostMoveBase Base { get; set; }

    public GhostMove(GhostMoveBase pBase)
    {
        Base = pBase;
    } 
}
