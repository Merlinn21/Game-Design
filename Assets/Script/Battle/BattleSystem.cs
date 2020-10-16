using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum BattleState
{
    Start, PlayerChoose, PlayerMove, PlayerBattle, EnemyMove, Busy
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private GhostParty[] party;
    [SerializeField] private GhostParty eventParty;
    [SerializeField] private BattleUnit[] enemies;
    [SerializeField] private BattleHud[] battleHuds;
    [SerializeField] private PlayerHud playerHud;
    [SerializeField] private PlayerMoveSet moveSet;
    [SerializeField] private BattleDialogue dialogueBox;

    private int targetNum;
    private List<Ghost> ghostTarget;
    public float waitDialogue = 1f;
    public GhostBase noGhost;

    private BattleState state;
    private bool change = false;

    //Untuk UI
    private int currentAction = 0;
    private int currentActionBattle = 0;
    private int currentActionBattleMove = 0;

    public event Action<bool> onBattleOver;

    public KeyCode confirmBtn;
    public KeyCode backBtn;

    public void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public IEnumerator SetupBattle()
    {
        targetNum = 0;
        dialogueBox.ActivateDialogue();
        state = BattleState.Start;
        ghostTarget = new List<Ghost>();
        for (int i = 0; i < enemies.Length; i++)
        {
            battleHuds[i].SetData(enemies[i].Ghost);
            battleHuds[i].Alive();
            ghostTarget.Add(enemies[i].Ghost);
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
        state = BattleState.PlayerChoose;
    }

    public void PlayerMove(int move)
    {
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
            dialogueBox.ActivateDialogue();
            yield return dialogueBox.TypeDialogue($"{enemies[targetNum].Ghost.Base.getName()} go brrrrrr");
            dialogueBox.CloseDialogue();
            yield return DeadEnemies(targetNum);
        }

        StartCoroutine(EnemyMove());
        
    }

    public void ChangeTarget(int number)
    {
        battleHuds[targetNum].DeactivateTarget();
        targetNum = number;
        Debug.Log(ghostTarget[number].Base.getName());
        battleHuds[targetNum].ActivateTarget();
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        
        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i].Ghost.Base.getName() == "NoGhost" || enemies[i].Ghost.alive == false)
            {
                continue;
            }
            //-------------------------------------Ghost Move-------------------------------------------
            var move = enemies[i].Ghost.getRandomMove();
            
            if(move.Base.getTargetType() == targetType.Enemy || move.Base.getTargetType() == targetType.Aoe)
            {
                int dmg = enemies[i].Ghost.GiveDmg(move.Base);
                dialogueBox.ActivateDialogue();
                yield return dialogueBox.TypeDialogue($"{enemies[i].Ghost.Base.getName()} used {move.Base.getMoveName()} for {dmg.ToString()} Damage");
            }



            //-------------------------------------Ghost Move-------------------------------------------

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
            state = BattleState.PlayerMove;
        }
        
    }

    IEnumerator DeadEnemies(int number)
    {
        enemies[number].Ghost.alive = false;
        battleHuds[number].Dead();
        ghostTarget.RemoveAt(number);
        
        for(int i = 0; i< ghostTarget.Count; i++)
        {
            if(ghostTarget[i].alive != false)
            {
                ChangeTarget(i);
            }
        }
        
        yield return new WaitForSeconds(.2f);
    }

    private void Update()
    {
        if(state == BattleState.PlayerChoose)
        {
            HandleActionSelection();
        }
        else if(state == BattleState.PlayerBattle)
        {
            HandleBattleSelection();
        }
        else if(state == BattleState.PlayerMove)
        {
            HandleBattleMoveSelection();
        }
    }

    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentAction < 3)
                currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentAction > 0)
                currentAction--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentAction < 2)
                currentAction += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentAction > 1)
                currentAction -= 2;
        }

        playerHud.UpdateChooseAction(currentAction);

        if (Input.GetKeyDown(confirmBtn))
        {
            if (currentAction == 0)
            {
                state = BattleState.PlayerBattle;
                playerHud.Fight();   
            }
            else if (currentAction == 2)
                RunAway();
        }        
    }

    private void HandleBattleSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentActionBattle < 3)
                currentActionBattle++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentActionBattle > 0)
                currentActionBattle--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentActionBattle < 2)
                currentActionBattle += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentActionBattle > 1)
                currentActionBattle -= 2;
        }

        playerHud.UpdateBattleAction(currentActionBattle);

        if (Input.GetKeyDown(confirmBtn))
        {
            if (currentActionBattle == 0)
            {
                //attack
                state = BattleState.PlayerBattle;
                playerHud.Fight();
            }
            else if(currentActionBattle == 1)
            {
                //skil;
                state = BattleState.PlayerMove;
                playerHud.OpenSkill();
            }
            else if (currentActionBattle == 2)
            {
                //defend
            }
            else if (currentActionBattle == 3)
            {
                //back
                state = BattleState.PlayerChoose;
                playerHud.CloseFight();
            }
                
        }
        else if (Input.GetKeyDown(backBtn))
        {
            state = BattleState.PlayerChoose;
            playerHud.CloseFight();
        }
    }

    private void HandleBattleMoveSelection()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentActionBattleMove < 3)
                currentActionBattleMove++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentActionBattleMove > 0)
                currentActionBattleMove--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentActionBattleMove < 2)
                currentActionBattleMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            if (currentActionBattleMove > 1)
                currentActionBattleMove -= 2;
        }

        playerHud.UpdateBattleMoveAction(currentActionBattleMove);

        if (Input.GetKeyDown(confirmBtn))
        {
            PlayerMove(currentActionBattleMove);
        }
        else if (Input.GetKeyDown(backBtn))
        {
            state = BattleState.PlayerBattle;
            playerHud.CloseSkill();
        }
    }


}
