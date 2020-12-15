using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerStatWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text mpText;
    [SerializeField] private TMP_Text maxHpText;
    [SerializeField] private TMP_Text maxMpText;
    [SerializeField] private TMP_Text atkText;
    [SerializeField] private TMP_Text defText;
    [SerializeField] private TMP_Text satkText;
    [SerializeField] private TMP_Text sdefText;
    [SerializeField] private TMP_Text expText;
    [SerializeField] private TMP_Text maxExpText;

    public void UpdateUI()
    {
        hpText.text = PlayerStat.health.ToString();
        mpText.text = PlayerStat.mana.ToString();
        maxHpText.text = PlayerStat.maxHealth.ToString();
        maxMpText.text = PlayerStat.maxMana.ToString();
        atkText.text = PlayerStat.atk.ToString();
        defText.text = PlayerStat.def.ToString();
        satkText.text = PlayerStat.satk.ToString();
        sdefText.text = PlayerStat.sdef.ToString();
        expText.text = PlayerStat.exp.ToString();
        maxExpText.text = PlayerStat.maxExp.ToString();
    }
}
