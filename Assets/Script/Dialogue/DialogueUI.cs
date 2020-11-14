using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text fullName;
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private Image portrait;
    [SerializeField] private float typeSpeed = 4f;
    [SerializeField] private DialogueTrigger trigger;
    private Character speaker;

    public Character Speaker
    {
        get { return speaker; }
        set
        {
            speaker = value;
            portrait.sprite = speaker.getPotrait();
            fullName.text = speaker.getFullName();
        }
    }

    public void Dialogue(string value)
    {
        StartCoroutine(TypeDialogue(value));
    }

    public bool HasSpeaker()
    {
        return speaker != null;
    }

    public bool SpeakerIs(Character character)
    {
        return speaker == character;
    }

    public void ShowUI()
    {
        gameObject.SetActive(true);
    }

    public void HideUI()
    {
        gameObject.SetActive(false);
    }

    public IEnumerator TypeDialogue(string dialogueText)
    {
        dialogue.text = "";
        foreach (var letter in dialogueText.ToCharArray())
        {
            dialogue.text += letter;
            yield return new WaitForSeconds(1f / typeSpeed);
        }
        yield return new WaitForSeconds(0.5f);
        trigger.typingState = TypingState.Finished;
    }
}
