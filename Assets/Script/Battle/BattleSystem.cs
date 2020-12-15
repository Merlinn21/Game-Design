using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum BattleState
{
    Start, PlayerChoose, PlayerMove, PlayerBattle, EnemyMove,
    Busy, ChooseTarget, ChooseTalkTarget, GhostDialogue, MultiChoice,
    Bonus, Journal
}

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private PlayerUnit player;
    [SerializeField] private GhostParty[] party;
    [SerializeField] private GhostParty eventParty;
    [SerializeField] private BattleUnit[] enemies;
    [SerializeField] private PlayerHud playerHud;
    [SerializeField] private PlayerMoveSet moveSet;
    [SerializeField] private BattleDialogue dialogueBox;

    [SerializeField] private List<BattleUnit> ghostTarget;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource battleAudio;
    [SerializeField] private AudioSource exploreAudio;
    [SerializeField] private AudioClip textSFX;
    [SerializeField] private AudioClip uiSFX;
    [SerializeField] private SimpleBlit blit;
    [SerializeField] private BattleTransition transition;
    public float waitDialogue = 1f;
    public BattleState state;
    private bool change = false;
    private bool attack = false;

    //Untuk UI
    private int currentAction = 0;
    private int currentActionBattle = 0;
    private int currentActionBattleMove = 0;
    private int currentChooseTarget;
    private int currentDialogue = 0;
    private int currentTalkTarget;
    private int currentMultiChoice;

    public event Action<bool, int> onBattleOver;

    public KeyCode confirmBtn;
    public KeyCode backBtn;

    private KeyCode right = KeyCode.RightArrow;
    private KeyCode left = KeyCode.LeftArrow;
    private KeyCode up = KeyCode.UpArrow;
    private KeyCode down = KeyCode.DownArrow;

    private bool eventBattle;
    private bool talkToGhost;
    private bool failToPersuade = false;

    private GhostDialogueBase dialogueBase;
    private int dialogueIndex = 0;

    private int totalExp;
    AudioScript audioScript = new AudioScript();

    public void Awake()
    {
        this.gameObject.SetActive(false);
    }

    public IEnumerator SetupBattle()
    {
        totalExp = 0;
        currentAction = 0;
        currentActionBattle = 0;
        currentActionBattleMove = 0;
        currentChooseTarget = 0;
        currentTalkTarget = 0;
        currentMultiChoice = 0;
        dialogueIndex = 0;
        talkToGhost = false;
        dialogueBox.ActivateDialogue();
        state = BattleState.Start;

        StartCoroutine(StartTransition());
        ghostTarget = new List<BattleUnit>();
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<BattleHud>().SetData(enemies[i].Ghost);
            enemies[i].GetComponent<BattleHud>().Alive();

            if (enemies[i].Ghost.Base.getName() != "NoGhost")
            {
                ghostTarget.Add(enemies[i]);
            }
        }

        yield return dialogueBox.TypeDialogue("Evil spirit gathered");
        yield return new WaitForSeconds(waitDialogue);
        dialogueBox.CloseDialogue();
        PlayerAction();
    }

    public IEnumerator StartTransition()
    {
        StartCoroutine(audioScript.FadeOut(exploreAudio, 0.3f));
        StartCoroutine(audioScript.FadeIn(battleAudio, 0.3f));

        blit.enabled = true;
        transition.fadeState = "in";
        yield return new WaitForSeconds(2.2f);
        blit.enabled = false;
    }

    public void StartBattle(bool _randomBattle)
    {
        eventBattle = false;
        playerHud.SetPlayerData();
        RandomizeGhost();
        StartCoroutine(SetupBattle());
    }

    public void StartEventBattle(GhostParty _ghostParty)
    {
        eventBattle = true;
        playerHud.SetPlayerData();
        eventParty = _ghostParty;
        SetEventParty();
        StartCoroutine(SetupBattle());
    }

    public void RunAway()
    {
        onBattleOver(true, 0);
    }

    private void RandomizeGhost()
    {
        int random = Mathf.FloorToInt(UnityEngine.Random.Range(0, party.Length));
        for (int i = 0; i < party[random].ghostParty.Length; i++)
        {
            int ghostLvl = UnityEngine.Random.Range(PlayerStat.lvl - 2, PlayerStat.lvl + 3);
            enemies[i].Setup(party[random].ghostParty[i], ghostLvl);
        }
    }

    private void SetEventParty()
    {
        for (int i = 0; i < eventParty.ghostParty.Length; i++)
        {
            int ghostLvl = eventParty.eventPartyLevels[i];
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

    //-----------------------------Battle------------------------------------------
    IEnumerator PerformPlayerMove(int moveNumber)
    {
        state = BattleState.Busy;

        var move = moveSet.getCurrentMoves()[moveNumber];
        audioSource.PlayOneShot(move.getMoveBase().getAudio());
        dialogueBox.ActivateDialogue();
        //------------------------Player Move-------------------------------
        targetType moveType = move.getMoveBase().getTargetType();

        if (moveType == targetType.Enemy)
        {
            player.PlayZoomInAnimation(ghostTarget[currentChooseTarget].GetPosition());
            ghostTarget[currentChooseTarget].PlayHitAnimation();

            int dmg = ghostTarget[currentChooseTarget].Ghost.TakeDmg(move.getMoveBase());
            ghostTarget[currentChooseTarget].GetComponent<BattleHud>().UpdateUI(ghostTarget[currentChooseTarget].Ghost);
            if (move.getMoveBase().getMoveName() != "Attack")
            {
                yield return dialogueBox.TypeDialogue($"Dara menggunakan {move.getMoveBase().getMoveName()} sebanyak {dmg.ToString()} Damage");
            }
            else
            {
                yield return dialogueBox.TypeDialogue($"Dara menyerang musuh sebanyak {dmg.ToString()} Damage");
            }

            player.PlayZoomOutAnimation();
        }
        if (moveType == targetType.Aoe)
        {
            int dmg = 0;

            for (int i = 0; i < ghostTarget.Count; i++)
            {
                ghostTarget[i].PlayHitAnimation();
                dmg = ghostTarget[i].Ghost.TakeDmg(move.getMoveBase());
                ghostTarget[i].GetComponent<BattleHud>().UpdateUI(ghostTarget[i].Ghost);
            }

            yield return dialogueBox.TypeDialogue($"Dara menyerang semua musuh dengan {dmg.ToString()} Damage");

        }
        if (moveType == targetType.Self)
        {
            //heal persen
            var heal = (move.getMoveBase().getBaseDmg() * PlayerStat.maxHealth) / 100;

            PlayerStat.health += (int)heal;

            if (PlayerStat.health >= PlayerStat.maxHealth)
            {
                PlayerStat.health = PlayerStat.maxHealth;
            }

            yield return dialogueBox.TypeDialogue($"Darah kamu pulih {heal.ToString()} HP");
        }

        //------------------------Player Move-------------------------------

        var moveBase = moveSet.getCurrentMoves()[moveNumber].getMoveBase();
        player.MinusCost(moveBase);


        for (int i = 0; i < ghostTarget.Count; i++)
        {
            if (ghostTarget[i].Ghost.HP <= 0)
            {        
                dialogueBox.ActivateDialogue();
                yield return dialogueBox.TypeDialogue($"{ghostTarget[i].Ghost.Base.getName()} berhasil ditaklukan");
                yield return DeadEnemies();
                i = 0;
            }
        }

        yield return new WaitForSeconds(waitDialogue);
        StartCoroutine(EnemyMove());

    }


    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;

        for (int i = 0; i < ghostTarget.Count; i++)
        {
            if (ghostTarget[i].Ghost.Base.getName() == "NoGhost" || enemies[i].Ghost.alive == false)
            {
                continue;
            }
            //-------------------------------------Ghost Move-------------------------------------------
            var move = ghostTarget[i].Ghost.getRandomMove();
            audioSource.PlayOneShot(move.Base.getAudio());
            //AOE or Single
            if (move.Base.getTargetType() == targetType.Enemy || move.Base.getTargetType() == targetType.Aoe)
            {
                state = BattleState.Busy;
                player.PlayHitAnimation();
                int dmg = ghostTarget[i].Ghost.GiveDmg(move.Base);
                dialogueBox.ActivateDialogue();
                yield return dialogueBox.TypeDialogue($"{ghostTarget[i].Ghost.Base.getName()} menggunakan {move.Base.getMoveName()} sebesar {dmg.ToString()} Damage");
            }

            if (move.Base.getTargetType() == targetType.Self)
            {
                state = BattleState.Busy;
                int heal = ghostTarget[i].Ghost.GiveHeal(move.Base);
                dialogueBox.ActivateDialogue();
                yield return dialogueBox.TypeDialogue($"{ghostTarget[i].Ghost.Base.getName()} menggunakan {move.Base.getMoveName()} dan pulih sebesar {heal.ToString()} HP");
            }

            //-------------------------------------Ghost Move-------------------------------------------

            state = BattleState.EnemyMove;
            playerHud.UpdateUI();
            ghostTarget[i].GetComponent<BattleHud>().UpdateUI(ghostTarget[i].Ghost);
            yield return new WaitForSeconds(waitDialogue);
            dialogueBox.CloseDialogue();
        }

        if (PlayerStat.health <= 0)
        {
            dialogueBox.ActivateDialogue();
            yield return dialogueBox.TypeDialogue("You died");
            onBattleOver(false, 0);
        }

        state = BattleState.PlayerBattle;
        ghostTarget[currentChooseTarget].GetComponent<BattleHud>().DeactivateTarget();
        playerHud.Fight();
        playerHud.CloseSkill();
        ghostTarget[currentChooseTarget].gameObject.GetComponent<BattleHud>().DeactivateTarget();
        currentChooseTarget = 0;

    }

    IEnumerator DeadEnemies()
    {
        BattleUnit deadGhost = ghostTarget[currentChooseTarget];

        totalExp += deadGhost.Ghost.GiveExp();

        deadGhost.Ghost.alive = false;
        deadGhost.GetComponent<BattleHud>().Dead();
        ghostTarget.RemoveAt(currentChooseTarget);
        if (ghostTarget.Count != 0)
            currentChooseTarget = 0;
        else
        {
            //TODO: SHOW +EXP 
            currentAction = 0;
            currentActionBattle = 0;
            currentActionBattleMove = 0;
            currentChooseTarget = 0;
            currentDialogue = 0;
            currentMultiChoice = 0;
            currentTalkTarget = 0;
            StartCoroutine(audioScript.FadeOut(battleAudio, 0.3f));
            StartCoroutine(audioScript.FadeIn(exploreAudio, 0.3f));

            onBattleOver(true, totalExp);
            Debug.Log(totalExp);
        }
        yield return new WaitForSeconds(.5f);
    }

    //-----------------------------Battle------------------------------------------

    //Handle State
    private void Update()
    {
        switch (state)
        {
            case BattleState.PlayerChoose:
                HandleActionSelection();
                break;
            case BattleState.Journal:
                if (Input.GetKeyDown(backBtn))
                {
                    state = BattleState.PlayerChoose;
                    playerHud.CloseJournal();
                }
                break;
            case BattleState.PlayerBattle:
                HandleBattleSelection();
                break;
            case BattleState.PlayerMove:
                HandleBattleMoveSelection();
                break;
            case BattleState.ChooseTarget:
                HandleChooseTargetSelection();
                break;
            case BattleState.ChooseTalkTarget:
                HandleTalkTarget();
                break;
            case BattleState.GhostDialogue:
                if (Input.GetKeyDown(confirmBtn))
                {
                    NextDialogue();
                }
                break;
            case BattleState.MultiChoice:
                HandleMultiChoice();
                break;
            case BattleState.Bonus:
                HandleBonusChoice();
                break;
            default:
                break;
        }

        if (Input.GetKeyDown(right) || Input.GetKeyDown(left) || Input.GetKeyDown(up) || Input.GetKeyDown(down) && state != BattleState.Busy)
            audioSource.PlayOneShot(uiSFX);
        
        if (Input.GetKeyDown(confirmBtn) || Input.GetKeyDown(backBtn) && state != BattleState.Busy)
            audioSource.PlayOneShot(textSFX);

    }

    //Action Panel
    private void HandleActionSelection()
    {
        if (Input.GetKeyDown(right))
        {
            if (currentAction < 3)
                currentAction++;
        }
        else if (Input.GetKeyDown(left))
        {
            if (currentAction > 0)
                currentAction--;
        }
        else if (Input.GetKeyDown(down))
        {
            if (currentAction < 2)
                currentAction += 2;
        }
        else if (Input.GetKeyDown(up))
        {
            if (currentAction > 1)
                currentAction -= 2;
        }

        playerHud.UpdateChooseAction(currentAction);

        if (Input.GetKeyDown(confirmBtn))
        {
            if (currentAction == 0)
            {
                //Battle
                state = BattleState.PlayerBattle;
                playerHud.Fight();   
            }
            else if(currentAction == 1)
            {
                //Talk
                state = BattleState.ChooseTalkTarget; 
            }
            else if (currentAction == 2)
            {
                //Run
                if(!eventBattle)
                    RunAway();
                else
                {
                    state = BattleState.Busy;
                    StartCoroutine(FailedToRun(BattleState.PlayerChoose)); 
                }
            }
            else if(currentAction == 3)
            {
                state = BattleState.Journal;
                playerHud.OpenJournal();
            }
        }        
    }

    //Battle Panel
    private void HandleBattleSelection()
    {
        if (Input.GetKeyDown(right))
        {
            if (currentActionBattle < 3)
                currentActionBattle++;
        }
        else if (Input.GetKeyDown(left))
        {
            if (currentActionBattle > 0)
                currentActionBattle--;
        }
        else if (Input.GetKeyDown(down))
        {
            if (currentActionBattle < 2)
                currentActionBattle += 2;
        }
        else if (Input.GetKeyDown(up))
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
                currentActionBattleMove = 4;
                state = BattleState.ChooseTarget;
                attack = true;
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
                currentAction = 0;
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

    //Move Panel
    private void HandleBattleMoveSelection()
    {
        ghostTarget[currentChooseTarget].GetComponent<BattleHud>().DeactivateTarget();

        if (Input.GetKeyDown(right))
        {
            if (currentActionBattleMove < 3)
                currentActionBattleMove++;
        }
        else if (Input.GetKeyDown(left))
        {
            if (currentActionBattleMove > 0)
                currentActionBattleMove--;
        }
        else if (Input.GetKeyDown(down))
        {
            if (currentActionBattleMove < 2)
                currentActionBattleMove += 2;
        }
        else if (Input.GetKeyDown(up))
        {
            if (currentActionBattleMove > 1)
                currentActionBattleMove -= 2;
        }

        playerHud.UpdateBattleMoveAction(currentActionBattleMove);

        if (Input.GetKeyDown(confirmBtn))
        {
            var moveType = moveSet.getCurrentMoves()[currentActionBattleMove].getMoveBase().getTargetType();
            if (moveType == targetType.Enemy)
            {
                state = BattleState.ChooseTarget;
            }
            else if (moveType == targetType.Aoe)
            {
                PlayerMove(currentActionBattleMove);
            }
            else if(moveType == targetType.Debuff)
            {
                state = BattleState.ChooseTarget;
            }
            else
            {
                PlayerMove(currentActionBattleMove);
            }
        }
        else if (Input.GetKeyDown(backBtn))
        {
            currentActionBattle = 0;
            state = BattleState.PlayerBattle;
            playerHud.CloseSkill();
        }
    }

    //Target Panel
    private void HandleChooseTargetSelection()
    {
        if(ghostTarget.Count != 1)
        {
            if (Input.GetKeyDown(right))
            {
                if(currentChooseTarget < ghostTarget.Count - 1)
                    currentChooseTarget++;
               
            }
            else if (Input.GetKeyDown(left))
            {
                if (currentChooseTarget > 0)
                    currentChooseTarget--;            
            }
        }
        else
        {
            currentChooseTarget = 0;
        }

        for (int i = 0; i < ghostTarget.Count; i++)
        {
            if (currentChooseTarget == i)
            {
                ghostTarget[i].GetComponent<BattleHud>().ActivateTarget();
            }
            else
            {
                ghostTarget[i].GetComponent<BattleHud>().DeactivateTarget();
            }
        }
        
        if (Input.GetKeyDown(confirmBtn))
        {
            PlayerMove(currentActionBattleMove);
        }
        else if (Input.GetKeyDown(backBtn))
        {
            currentActionBattleMove = 0;
            ghostTarget[currentChooseTarget].gameObject.GetComponent<BattleHud>().DeactivateTarget();

            if (attack)
            {
                state = BattleState.PlayerBattle;
                attack = false;
            }
            else
                state = BattleState.PlayerMove;            
        }
    }

    //Choose Talk Target Panel
    private void HandleTalkTarget()
    {
        if (ghostTarget.Count != 1)
        {
            if (Input.GetKeyDown(right))
            {
                if (currentTalkTarget < ghostTarget.Count - 1)
                    currentTalkTarget++;

            }
            else if (Input.GetKeyDown(left))
            {
                if (currentTalkTarget > 0)
                    currentTalkTarget--;
            }
        }
        else
        {
            currentTalkTarget = 0;
        }

        for (int i = 0; i < ghostTarget.Count; i++)
        {
            if (currentTalkTarget == i)
            {
                ghostTarget[i].GetComponent<BattleHud>().ActivateTarget();
            }
            else
            {
                ghostTarget[i].GetComponent<BattleHud>().DeactivateTarget();
            }
        }

        if (Input.GetKeyDown(confirmBtn))
        {
            //TODO: Start Talking
            if (!talkToGhost)
            {
                talkToGhost = true;
                StartGhostDialogue(currentTalkTarget);
            }
            else if(talkToGhost)
            {
                dialogueBox.ActivateDialogue();
                if(failToPersuade)
                    StartCoroutine(TypeDialogue(BattleState.ChooseTalkTarget, "The Ghost are enraged!"));
                else if(!failToPersuade || eventBattle)
                    StartCoroutine(TypeDialogue(BattleState.ChooseTalkTarget ,"The Ghost are not Willing to Talk"));
            }

        }
        else if (Input.GetKeyDown(backBtn))
        {
            currentActionBattleMove = 0;
            ghostTarget[currentTalkTarget].gameObject.GetComponent<BattleHud>().DeactivateTarget();
            currentTalkTarget = 0;
            
            state = BattleState.PlayerChoose;
        }
    }

    //Ghost Multi Choice Panel
    private void HandleMultiChoice()
    {
        if (Input.GetKeyDown(down))
        {
            if (currentMultiChoice < 1)
                currentMultiChoice++;
        }
        else if (Input.GetKeyDown(up))
        {
            if (currentMultiChoice > 0)
                currentMultiChoice--;
        }

        playerHud.UpdateMultiChoiceAction(currentMultiChoice);

        if (Input.GetKeyDown(confirmBtn))
        {
            NextMultiDialogue();
            playerHud.CloseMulti();
        }
    }

    //Choose Bonus Panel
    private void HandleBonusChoice()
    {
        if (Input.GetKeyDown(confirmBtn))
        {
            if (dialogueBase.ghostLines[dialogueIndex].success)
            {
                //Berhasil Persuade
                //TODO: Get bonus
                StartCoroutine(StartBonus("You Successfully persuaded the ghost!", dialogueBase.bonusType, dialogueBase.bonus));
                PlayerBonus(dialogueBase.bonusType, dialogueBase.bonus);
                StartCoroutine(PersuadeSucces());
                playerHud.UpdateUI();
            }
            else
            {
                //Gagal Persuade
                failToPersuade = true;
                StartCoroutine(EndTalk("U Fail :("));
            }
        }
    }


    //-------------------------------Dialogue------------------------------------------------

    private void StartGhostDialogue(int talkTarget)
    {
        player.PlayZoomInAnimation(ghostTarget[talkTarget].GetPosition());
        currentMultiChoice = 0;
        dialogueBox.ActivateDialogue();
        GhostBase ghost = ghostTarget[talkTarget].Ghost.Base;
        int dialogueSize = ghost.getGhostDialogue().Count;

        int randomIndex = UnityEngine.Random.Range(0, dialogueSize);
        dialogueBase = ghostTarget[talkTarget].Ghost.Base.getGhostDialogue()[randomIndex];

        if (dialogueBase.ghostLines[dialogueIndex].multiChoices.Length > 0)
        {
            //TODO: Show choice
            state = BattleState.MultiChoice;
            StartCoroutine(TypeDialogue(BattleState.MultiChoice, dialogueBase.ghostLines[dialogueIndex].text));
            playerHud.OpenMulti();
            playerHud.UpdateChoice(dialogueBase.ghostLines[dialogueIndex].multiChoices);
        }
        else
        {
            StartCoroutine(TypeDialogue(BattleState.GhostDialogue, dialogueBase.ghostLines[dialogueIndex].text));
        }
    }

    private void NextDialogue()
    {
        if (dialogueBase.ghostLines[dialogueIndex].nextText > 0)
        {
            dialogueIndex = dialogueBase.ghostLines[dialogueIndex].nextText;
        }
       
        GhostLine ghostLine = dialogueBase.ghostLines[dialogueIndex];
        audioSource.PlayOneShot(textSFX);
        audioSource.PlayOneShot(textSFX);
        audioSource.PlayOneShot(textSFX);
        if (ghostLine.multiChoices.Length > 0)
        {
            //TODO: Show Stats Update
            state = BattleState.MultiChoice;
            StartCoroutine(TypeDialogue(BattleState.MultiChoice, ghostLine.text));
            playerHud.OpenMulti();
            playerHud.UpdateChoice(ghostLine.multiChoices);
        }
        else
        {
            if(ghostLine.nextText == -1 && ghostLine.end == true)
            {
                StartCoroutine(TypeDialogue(BattleState.Bonus, ghostLine.text));
            }
            else
            {
                StartCoroutine(TypeDialogue(BattleState.GhostDialogue, ghostLine.text));

            }
        }
    }

    IEnumerator TypeDialogue(BattleState prevState, String text)
    {
        state = BattleState.Busy;
        yield return dialogueBox.TypeGhostDialogue(text);
        state = prevState;
        if (prevState == BattleState.ChooseTalkTarget)
            dialogueBox.CloseDialogue();
    }

    IEnumerator EndTalk(string text)
    {
        state = BattleState.Busy;
        player.PlayZoomOutAnimation();
        yield return dialogueBox.TypeGhostDialogue(text);
        yield return new WaitForSeconds(1f);
        state = BattleState.PlayerChoose;
        dialogueBox.CloseDialogue();
    }

    IEnumerator StartBonus(string text, BonusStat bonusStat, int bonusValue)
    {
        state = BattleState.Busy;
        yield return dialogueBox.TypeGhostDialogue(text);
        yield return new WaitForSeconds(1f);
        yield return dialogueBox.TypeGhostDialogue($"+{bonusValue} {bonusStat.ToString()}");
        yield return new WaitForSeconds(1f);
        dialogueBox.CloseDialogue();
        if (ghostTarget.Count > 0)
            state = BattleState.PlayerChoose;
        else
            onBattleOver(true, totalExp);

    }

    IEnumerator FailedToRun(BattleState prevState)
    {
        dialogueBox.ActivateDialogue();
        yield return dialogueBox.TypeDialogue("Dark power prevents your escapes.");
        state = prevState;
    }

    private void NextMultiDialogue()
    {
        StartCoroutine(TypeDialogue(BattleState.GhostDialogue, dialogueBase.ghostLines[dialogueIndex].multiChoices[currentMultiChoice].responseText));
        dialogueIndex = dialogueBase.ghostLines[dialogueIndex].multiChoices[currentMultiChoice].nextText;
    }

    private void PlayerBonus(BonusStat statType, int value)
    {
        switch (statType)
        {
            case BonusStat.MP:
                PlayerStat.maxMana += value;
                break;
            case BonusStat.HP:
                PlayerStat.maxHealth += value;
                break;
            case BonusStat.Attack:
                PlayerStat.atk += value;
                break;
            case BonusStat.Defense:
                PlayerStat.def += value;
                break;
            case BonusStat.SAttack:
                PlayerStat.satk += value;
                break;
            case BonusStat.SDefense:
                PlayerStat.sdef += value;
                break;
        }
    }

    IEnumerator PersuadeSucces()
    {
        player.PlayZoomOutAnimation();
        ghostTarget[currentTalkTarget].Ghost.alive = false;
        ghostTarget[currentTalkTarget].GetComponent<BattleHud>().Dead();
        ghostTarget.RemoveAt(currentTalkTarget);
        if (ghostTarget.Count != 0)
            currentTalkTarget = 0;
        yield return new WaitForSeconds(.5f);
    }

    //-------------------------------Dialogue------------------------------------------------

}
