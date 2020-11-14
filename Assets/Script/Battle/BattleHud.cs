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
    [SerializeField] private GameObject lvl;
    [SerializeField] private GameObject hp;
    [SerializeField] private Image arrowTarget;
    [SerializeField] private Color originalColor;
    [SerializeField] private Color highlighColor;


    public void SetData(Ghost ghost)
    {
        if(ghost != null)
        {
            lvlText.text = ghost.Level.ToString();
            hpText.text = ghost.HP.ToString();
        }
    }

    public void ActivateTarget()
    {
        arrowTarget.enabled = true;
        this.gameObject.GetComponent<Image>().color = highlighColor;
    }

    public void DeactivateTarget()
    {
        arrowTarget.enabled = false;
        this.gameObject.GetComponent<Image>().color = originalColor;

    }

    public void UpdateUI(Ghost ghost)
    {
        hpText.text = ghost.HP.ToString();
    }

    public void Dead()
    {
        DeactivateTarget();
        this.gameObject.GetComponent<Image>().enabled = false;
        lvl.SetActive(false);
        hp.SetActive(false);
    }
    public void Alive()
    {
        this.gameObject.GetComponent<Image>().enabled = true;
        lvl.SetActive(true);
        arrowTarget.enabled = false;
        hp.SetActive(true);
    }
}
