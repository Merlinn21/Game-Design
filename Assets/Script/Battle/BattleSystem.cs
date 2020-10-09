using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum BattleState
{
    Start, PlayerAction, PlayerMove, EnemyMove, Busy
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GhostParty[] party;
    [SerializeField] private GhostParty eventParty;
    [SerializeField] private BattleUnit[] enemies;
    [SerializeField] private BattleHud[] battleHuds;
    [SerializeField] private PlayerHud playerHud;
    [SerializeField] private PlayerMoveSet moveSet;
    private int targetNum;
    public float waitDialogue = 1f;
    public GhostBase noGhost;

    [SerializeField] private BattleDialogue dialogueBox;
    private BattleState state;
    private bool change = false;

    public event Action<bool> onBattleOver;

    public void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public IEnumerator SetupBattle()
    {
        targetNum = 0;
        dialogueBox.ActivateDialogue();
        state = BattleState.Start;

        for(int i = 0; i < enemies.Length; i++)
        {
            battleHuds[i].SetData(enemies[i].Ghost);
        }
        battleHuds[targetNum].ActivateTarget();

        yield return dialogueBox.TypeDialogue("Evil spirit gathered");
        yield return new WaitForSeconds(waitDialogue);
        dialogueBox.CloseDialogue();


        PlayerAction();
    }

    public void StartBattle(bool _randomBattle)
    {
        playerHud.SetPlayerData();
        RandomizeGhost();
        StartCoroutine(SetupBattle());
    }

    public void StartEventBattle(GhostParty _ghostParty)
    {
        playerHud.SetPlayerData();
        eventParty = _ghostParty;
        SetEventParty();
        StartCoroutine(SetupBattle());
    }

    public void RunAway()
    {
        onBattleOver(true);
    }

    private void RandomizeGhost()
    {
        int random = Mathf.FloorToInt(UnityEngine.Random.Range(0, party.Length));
        for(int i = 0; i < party[random].ghostParty.Length; i++)
        {
            int ghostLvl = UnityEngine.Random.Range(party[random].minLvl, party[random].maxLvl + 1);
            enemies[i].Setup(party[random].ghostParty[i], ghostLvl);
        }
    }

    private void SetEventParty()
    {
        for (int i = 0; i < eventParty.ghostParty.Length; i++)
        {
            int ghostLvl = UnityEngine.Random.Range(party[i].minLvl, party[i].maxLvl);
            enemies[i].Setup(eventParty.ghostParty[i], ghostLvl);
        }
    }

    public void PlayerAction()
    {
        state = BattleState.PlayerAction;
    }

    public void PlayerMove(int move)
    {
        state = BattleState.PlayerMove;
        StartCoroutine(PerformPlayerMove(move));
    }

    IEnumerator PerformPlayerMove(int moveNumber)
    {
        state = BattleState.Busy;

        var move = moveSet.getCurrentMoves()[moveNumber];
        dialogueBox.ActivateDialogue();

        int dmg = enemies[targetNum].Ghost.TakeDmg(move.getMoveBase());
        yield return dialogueBox.TypeDialogue($"You used {move.getMoveBase().getMoveName()} for {dmg.ToString()} Damage");
        battleHuds[targetNum].UpdateUI(enemies[targetNum].Ghost);
        yield return new WaitForSeconds(waitDialogue);

        dialogueBox.CloseDialogue();

        if (enemies[targetNum].Ghost.HP <= 0)
        {
            yield return dialogueBox.TypeDialogue($"{enemies[targetNum].Ghost.Base.getName()} go brrrrrr");
            yield return DeadEnemies(targetNum);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    public void ChangeTarget(int number)
    {
        battleHuds[targetNum].DeactivateTarget();
        targetNum = number;
        battleHuds[targetNum].ActivateTarget();
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].Ghost.Base.getName() == "NoGhost" || enemies[i].Ghost.alive == false)
            {
                break;
            }
            var move = enemies[i].Ghost.getRandomMove();

            dialogueBox.ActivateDialogue();
            int dmg = enemies[i].Ghost.GiveDmg(move.Base);
            yield return dialogueBox.TypeDialogue($"{enemies[i].Ghost.Base.getName()} used {move.Base.getMoveName()} for {dmg.ToString()} Damage");
            playerHud.UpdateUI();
            yield return new WaitForSeconds(waitDialogue);
            dialogueBox.CloseDialogue();
        }
        if (PlayerStat.health <= 0)
        {
            yield return dialogueBox.TypeDialogue("Ded");
        }
        else
        {
            PlayerAction();
        }
        
    }

    IEnumerator DeadEnemies(int number)
    {
        enemies[number].Ghost.alive = false;
        battleHuds[number].Dead();
        yield return new WaitForSeconds(1f);
    }
}
