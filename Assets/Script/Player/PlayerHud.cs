using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    [SerializeField] private PlayerMoveSet moveSet;
    [Space]
    [Space]

    [Header("Stat HUD")]
    [SerializeField] private TMP_Text playerHP;
    [SerializeField] private TMP_Text playerMP;
    [SerializeField] private TMP_Text maxPlayerHP;
    [SerializeField] private TMP_Text maxPlayerMP;

    [Space]
    [Header("Move HUD")]
    [Space]
    [SerializeField] private TMP_Text move1;
    [SerializeField] private TMP_Text move2;
    [SerializeField] private TMP_Text move3;
    [SerializeField] private TMP_Text move4;

    [Space]
    [Header("UnityEvent")]
    [Space]
    public UnityEvent onFight;
    public UnityEvent onFightClose;
    public UnityEvent onSkillOpen;
    public UnityEvent onSkillClose;

    [Space]
    [Header("Button List")]
    [Space]
    [SerializeField] private List<Button> actionList;
    [SerializeField] private List<Button> fightList;
    [SerializeField] private List<Button> moveList;

    public void SetPlayerData()
    {
        playerHP.text = PlayerStat.health.ToString();
        playerMP.text = PlayerStat.mana.ToString();
        maxPlayerHP.text = PlayerStat.maxHealth.ToString();
        maxPlayerMP.text = PlayerStat.maxMana.ToString();

        move1.text = moveSet.getCurrentMoves()[0].getMoveBase().getMoveName().ToString();
        move2.text = moveSet.getCurrentMoves()[1].getMoveBase().getMoveName().ToString();
        move3.text = moveSet.getCurrentMoves()[2].getMoveBase().getMoveName().ToString();
        move4.text = moveSet.getCurrentMoves()[3].getMoveBase().getMoveName().ToString();

    }

    public void Fight(){ onFight.Invoke();}
    public void CloseFight() { onFightClose.Invoke(); }
    public void OpenSkill(){ onSkillOpen.Invoke();}
    public void CloseSkill(){ onSkillClose.Invoke();}

    public void UpdateUI()
    {
        playerHP.text = PlayerStat.health.ToString();
        playerMP.text = PlayerStat.mana.ToString();
    }

    public void UpdateChooseAction(int index)
    {
        for(int i = 0; i<actionList.Count; i++)
        {
            if (i == index)
            {
                actionList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
            }
            else
            {
                actionList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
            }
        }
    }

    public void UpdateBattleAction(int index){
        for (int i = 0; i < actionList.Count; i++)
        {
            if (i == index)
            {
                fightList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
            }
            else
            {
                fightList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
            }
        }
    }

    public void UpdateBattleMoveAction(int index)
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            if (i == index)
            {
                moveList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
            }
            else
            {
                moveList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
            }
        }
    }
}
