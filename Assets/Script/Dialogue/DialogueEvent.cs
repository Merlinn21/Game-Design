using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvent : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField] private DialogueTrigger trigger;
    public GhostParty ghostParty;

    public void Deactivate()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Encounter");
    }
}
