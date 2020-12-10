using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueScene : MonoBehaviour
{
    [SerializeField] private Dialogue startDialogue;
    [SerializeField] private DialogueTrigger dialogueTrigger;
    AudioScript audioScript = new AudioScript();
    public AudioSource audioSource;

    private void Start()
    {
        audioScript.FadeIn(audioSource, 0.3f);
        StartCoroutine(StartDialogue());
    }

    IEnumerator StartDialogue()
    {
        yield return new WaitForSeconds(1f);
        dialogueTrigger.SetDialogue(startDialogue);
        dialogueTrigger.StartDialogue();
    }
    private void Update()
    {
        dialogueTrigger.HandleUpdate();
    }
}
