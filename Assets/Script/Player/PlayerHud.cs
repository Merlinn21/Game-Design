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
    [SerializeField] private List<GameObject> moves;

    [Space]
    [Header("UnityEvent")]
    [Space]
    public UnityEvent onFight;
    public UnityEvent onFightClose;
    public UnityEvent onSkillOpen;
    public UnityEvent onSkillClose;
    public UnityEvent onMultiOpen;
    public UnityEvent onMultiClose;

    [Space]
    [Header("Button List")]
    [Space]
    [SerializeField] private List<Button> actionList;
    [SerializeField] private List<Button> fightList;
    [SerializeField] private List<Button> moveList;
    [SerializeField] private List<GameObject> multiChoice;

    public void SetPlayerData()
    {
        playerHP.text = PlayerStat.health.ToString();
        playerMP.text = PlayerStat.mana.ToString();

        maxPlayerHP.text = PlayerStat.maxHealth.ToString();
        maxPlayerMP.text = PlayerStat.maxMana.ToString();

        for(int i =  0; i < moves.Count; i++)
        {
            moves[i].transform.GetChild(0).GetComponent<TMP_Text>().text = moveSet.getCurrentMoves()[i].getMoveBase().getMoveName().ToString();
            moves[i].transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = moveSet.getCurrentMoves()[i].getMoveBase().GetMoveType().ToString();

            if (moveSet.getCurrentMoves()[i].getMoveBase().getCostType() == costType.Hp)
            {
                var cost = Mathf.Round(moveSet.getCurrentMoves()[i].getMoveBase().getCost() + 1 * PlayerStat.health / 100); 
                moves[i].transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = cost.ToString() + " Hp";
            }
            else
            {
                moves[i].transform.GetChild(1).GetChild(3).GetComponent<TMP_Text>().text = moveSet.getCurrentMoves()[i].getMoveBase().getCost().ToString() + " Mana";
            }
        }
    }

    public void Fight(){ onFight.Invoke();}
    public void CloseFight() { onFightClose.Invoke(); }
    public void OpenSkill(){ onSkillOpen.Invoke();}
    public void CloseSkill(){ onSkillClose.Invoke();}
    public void OpenMulti() { onMultiOpen.Invoke(); }
    public void CloseMulti() { onMultiClose.Invoke(); }

    public void UpdateUI()
    {
        playerHP.text = PlayerStat.health.ToString();
        playerMP.text = PlayerStat.mana.ToString();
        maxPlayerHP.text = PlayerStat.maxHealth.ToString();
        maxPlayerMP.text = PlayerStat.maxMana.ToString();
    }

    public void UpdateChooseAction(int index)
    {
        for(int i = 0; i<actionList.Count; i++)
        {
            if (i == index)
            {
                actionList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
                actionList[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                actionList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
                actionList[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateBattleAction(int index){
        for (int i = 0; i < actionList.Count; i++)
        {
            if (i == index)
            {
                fightList[i].GetComponentInChildren<TMP_Text>().color = Color.blue;
                fightList[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                fightList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
                fightList[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);

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
                moveList[i].gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                moveList[i].GetComponentInChildren<TMP_Text>().color = Color.black;
                moveList[i].gameObject.transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }

    public void UpdateChoice(MultiChoice[] choices)
    {
        for(int i = 0; i< choices.Length; i++)
        {
            multiChoice[i].GetComponent<TMP_Text>().text = choices[i].choiceText;
        }
    }

    public void UpdateMultiChoiceAction(int index)
    {
        for (int i = 0; i < multiChoice.Count; i++)
        {
            if (i == index)
            {
                multiChoice[i].GetComponent<TMP_Text>().color = Color.blue;
            }
            else
            {
                multiChoice[i].GetComponent<TMP_Text>().color = Color.white;
            }
        }
    }
}
