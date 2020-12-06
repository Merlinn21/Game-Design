using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public Animator anim;
    public void StartBattleTransition()
    {
        anim.SetTrigger("Start");
    }

}
