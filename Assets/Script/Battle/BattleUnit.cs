using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public GhostBase ghostBase;
    public int level;

    public Ghost Ghost { get; set; } //properties

    public void Setup(GhostBase ghost)
    {
        Ghost = new Ghost(ghost, level);

        if(Ghost != null)
        {
            GetComponent<Image>().sprite = Ghost.Base.getSprite();
            this.gameObject.SetActive(true);

        }
        
    }
}
