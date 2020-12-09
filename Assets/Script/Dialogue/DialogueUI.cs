using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text fullName;
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private TMP_Text dialogue2;
    [SerializeField] private GameObject nameBox;
    [SerializeField] private GameObject portraitBox;
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
            if(speaker.getPotrait() != null)
            {
                nameBox.SetActive(true);
                portraitBox.SetActive(true);
                dialogue.enabled = true;
                dialogue2.enabled = false;
                portrait.sprite = speaker.getPotrait();
                fullName.text = speaker.getFullName();
            }              
            else
            {
                dialogue.enabled = false;
                dialogue2.enabled = true;
                nameBox.SetActive(false);
                portraitBox.SetActive(false);
            }
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
        if(speaker.getPotrait() != null)
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
        else
        {
            dialogue2.text = "";
            foreach (var letter in dialogueText.ToCharArray())
            {
                dialogue2.text += letter;
                yield return new WaitForSeconds(1f / typeSpeed);
            }
            yield return new WaitForSeconds(0.5f);
            trigger.typingState = TypingState.Finished;
        }

        
    }
}
