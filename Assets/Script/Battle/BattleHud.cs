using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class BattleHud : MonoBehaviour
{
    [SerializeField] private TMP_Text lvlText;
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text targetText;
    [SerializeField] private GameObject lvl;
    [SerializeField] private GameObject hp;


    public void SetData(Ghost ghost)
    {
        targetText.enabled = false;
        if(ghost != null)
        {
            lvlText.text = ghost.Level.ToString();
            hpText.text = ghost.HP.ToString();
        }
    }

    public void ActivateTarget()
    {
        targetText.enabled = true;
    }

    public void DeactivateTarget()
    {
        targetText.enabled = false;
    }

    public void UpdateUI(Ghost ghost)
    {
        hpText.text = ghost.HP.ToString();
    }

    public void Dead()
    {
        this.gameObject.GetComponent<Image>().enabled = false;
        targetText.enabled = false;
        lvl.SetActive(false);
        hp.SetActive(false);
    }
    public void Alive()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        lvl.SetActive(true);
        hp.SetActive(true);
    }
}
