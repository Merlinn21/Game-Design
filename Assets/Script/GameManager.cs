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
    [SerializeField] private GameObject battleTransition;
    [SerializeField] private AudioSource exploreAudio;
    [SerializeField] private AudioSource battleAudio;

    AudioScript audioScript = new AudioScript();

    public GameState state;

    private void Start()
    {
        playerMove.onEncounter += StartBattle; // manggil fungsi StartBattle kalo onEncounter kepanggil
        battleSystem.onBattleOver += EndBattle;
        playerMove.onDialogue += StartDialogue;
        dialogueTrigger.onDialogueOver += EndDialogue;


        StartCoroutine(audioScript.FadeIn(exploreAudio, 0.5f));
        battleTransition.SetActive(false);
        StartCoroutine(FirstDialogue());
    }

    IEnumerator FirstDialogue()
    {
        yield return new WaitForSeconds(1.5f);
        StartDialogue(startDialogue);
    }

    public void StartBattle(bool randomBattle)
    {
        battleTransition.SetActive(true);

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
        AudioScript audioScript = new AudioScript();
        StartCoroutine(audioScript.FadeOut(battleAudio, 0.3f));
        StartCoroutine(audioScript.FadeIn(exploreAudio, 0.3f));
        battleTransition.SetActive(false);
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
            battleTransition.SetActive(true);
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
