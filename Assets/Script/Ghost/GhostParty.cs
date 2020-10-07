using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GhostParty", menuName = "Ghost/Create new Ghost party")]

public class GhostParty : ScriptableObject
{
    public GhostBase[] ghostParty;

}
