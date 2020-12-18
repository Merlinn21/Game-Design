using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Dialogue dialogue;
    [SerializeField] private DialogueTrigger trigger;
    [SerializeField] private GameManager gm;

    public void StartDialogue()
    {
        gm.state = GameState.Dialogue;
        Debug.Log(dialogue.name);
        trigger.SetDialogue(dialogue);
       
        trigger.StartDialogue();
    }

    public void JalangkungDialogue()
    {
        gm.state = GameState.Dialogue;
        trigger.SetDialogue(dialogue);

        PlayerStat.health = PlayerStat.maxHealth;
        PlayerStat.mana = PlayerStat.maxMana;
        trigger.StartDialogue();
    }
}
