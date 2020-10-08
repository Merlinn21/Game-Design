using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleHud : MonoBehaviour
{
    public TMP_Text lvlText;

    public void SetData(Ghost ghost)
    {
        if(ghost != null)
        {
            lvlText.text = ghost.Level.ToString();
        }
    }

}
