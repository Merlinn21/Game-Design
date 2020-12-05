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
        trigger.SetDialogue(dialogue);
        trigger.StartDialogue();
    }
}
