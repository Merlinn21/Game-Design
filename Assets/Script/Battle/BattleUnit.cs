using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUnit : MonoBehaviour
{
    public Ghost Ghost { get; set; } //properties
    public int level;

    public void Setup(GhostBase ghost, int lvl)
    {
        level = lvl;
        Ghost = new Ghost(ghost, level);

        if(Ghost != null)
        {
            GetComponent<Image>().sprite = Ghost.Base.getSprite();
            this.gameObject.SetActive(true);
        }

        if(Ghost.Base.getName() == "NoGhost")
        {
            this.gameObject.SetActive(false);
        }
    }

    public void ClearGhost()
    {
        Ghost = new Ghost(null, 0);
    }
}
