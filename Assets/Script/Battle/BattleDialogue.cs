using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class BattleDialogue : MonoBehaviour
{
    [SerializeField] TMP_Text dialogueText;
    public float typeSpeed;

    public UnityEvent onDialogueClose;

    public void setDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public IEnumerator TypeDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach(var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / typeSpeed);
        }
        yield return new WaitForSeconds(0.5f);
        CloseDialogue();
    }

    public IEnumerator TypeGhostDialogue(string dialogue)
    {
        dialogueText.text = "";
        foreach (var letter in dialogue.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(1f / typeSpeed);
        }
    }


    public void ActivateDialogue()
    {
        this.gameObject.SetActive(true);
    }

    public void CloseDialogue()
    {
        onDialogueClose.Invoke();
    }
}
