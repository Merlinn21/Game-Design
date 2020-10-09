using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSet : MonoBehaviour
{
    public LearnableMoves[] moveSetList;
    private LearnableMoves[] currentMoves;

    public LearnableMoves[] getCurrentMoves()
    {
        currentMoves = new LearnableMoves[5];
        currentMoves[0] = moveSetList[PlayerStat.move1];
        currentMoves[1] = moveSetList[PlayerStat.move2];
        currentMoves[2] = moveSetList[PlayerStat.move3];
        currentMoves[3] = moveSetList[PlayerStat.move4];
        currentMoves[4] = moveSetList[4]; // normal attack

        return currentMoves;
    }
}

[System.Serializable]
public class LearnableMoves
{
    [SerializeField] GhostMoveBase moveBase;

    public GhostMoveBase getMoveBase() { return moveBase; }
}


