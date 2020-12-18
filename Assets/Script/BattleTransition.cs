using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTransition : MonoBehaviour
{
    SimpleBlit blit;
    public float val = 1;
    public string fadeState = "none";

    void Start()
    {
        blit = GetComponent<SimpleBlit>();
        fadeState = "out";
    }

    void Update()
    {
        if(fadeState == "out")
        {
            val -= Time.deltaTime * 0.5f;
            val = Mathf.Clamp01(val);
            blit.TransitionMaterial.SetFloat("_Cutoff", val);
            blit.TransitionMaterial.SetFloat("_Fade", Mathf.Clamp01(val * 2));
        }
        else if(fadeState == "in")
        {
            val += Time.deltaTime * 0.5f;
            val = Mathf.Clamp01(val);
            blit.TransitionMaterial.SetFloat("_Cutoff", val);
            blit.TransitionMaterial.SetFloat("_Fade", Mathf.Clamp01(val * 1.8f));
        }

        if (val == 0 || val == 1)
        {
            fadeState = "none";
        }
    }
}
