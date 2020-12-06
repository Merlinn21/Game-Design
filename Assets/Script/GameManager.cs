using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    FreeRoam,
    Battle,
    Transition,
    Dialogue
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private GridMovement playerMove;
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Dialogue startDialogue;

    public GameState state;

    private void Start()
    {
        playerMove.onEncounter += StartBattle; // manggil fungsi StartBattle kalo onEncounter kepanggil
        battleSystem.onBattleOver += EndBattle;
        playerMove.onDialogue += StartDialogue;
        dialogueTrigger.onDialogueOver += EndDialogue;

        StartDialogue(startDialogue);
    }

    public void StartBattle(bool randomBattle)
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        battleSystem.transform.GetChild(0).gameObject.SetActive(true);
        battleSystem.transform.GetChild(1).gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        battleSystem.StartBattle(randomBattle);
    }
 
    public void EndBattle(bool win)
    {
        state = GameState.FreeRoam;
        Debug.Log(PlayerStat.exp);
        battleSystem.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        state = GameState.Dialogue;
        dialogueTrigger.SetDialogue(dialogue);
        dialogueTrigger.StartDialogue();
    }

    public void EndDialogue(bool freeRoam, GhostParty ghostParty) 
    {
        if (freeRoam)
        {
            state = GameState.FreeRoam;
            playerMove.onEventFalse();
        }
        else
        {
            state = GameState.Battle;
            battleSystem.gameObject.SetActive(true);
            battleSystem.transform.GetChild(0).gameObject.SetActive(true);
            battleSystem.transform.GetChild(1).gameObject.SetActive(true);
            mainCamera.gameObject.SetActive(false);
            playerMove.onEventFalse();
            battleSystem.StartEventBattle(ghostParty);
        }
    }


    private void Update()
    {
        if(state == GameState.FreeRoam)
        {
            playerMove.HandleUpdate();
        }
        else if(state == GameState.Battle)
        {

        }
        else if(state == GameState.Dialogue)
        {
            dialogueTrigger.HandleUpdate();
        }
    }
}
