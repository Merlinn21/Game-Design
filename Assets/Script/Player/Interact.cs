using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameManager gm;
    [SerializeField] private KeyCode interactButton = KeyCode.Z;
    [SerializeField] private GameObject interactPanel;


    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            interactPanel.SetActive(true);
        }

        if (other.CompareTag("Object") && Input.GetKeyDown(interactButton))
        {
            ObjectTrigger obj = other.gameObject.GetComponent<ObjectTrigger>();
            gm.StartDialogue(obj.dialogue);

            if (obj.isKey)
            {
                obj.door.SetActive(false);
                other.gameObject.SetActive(false);
            }

        }

        if (other.CompareTag("Door") || other.CompareTag("Stair") && Input.GetKeyDown(interactButton) && gm.state == GameState.FreeRoam)
        {        
            Door door = other.gameObject.GetComponent<Door>();
            door.StartDialogue();
        }

        if (other.CompareTag("Jalangkung") && Input.GetKeyDown(interactButton) && gm.state == GameState.FreeRoam)
        {
            Door door = other.gameObject.GetComponent<Door>();
            door.JalangkungDialogue();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Object"))
        {
            interactPanel.SetActive(false);
        }
    }
}
