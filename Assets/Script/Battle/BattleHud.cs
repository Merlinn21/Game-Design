using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    public Text ghostName;

    public void SetData(Ghost ghost)
    {
        if(ghost != null)
        {
            ghostName.text = ghost.Base.getName();
            
        }
    }
}
