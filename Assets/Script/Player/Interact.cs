using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private GameManager gm; 
    [SerializeField] private KeyCode interactButton = KeyCode.F;


    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Object") && Input.GetKeyDown(interactButton))
        {
            //TODO: Open Door
            ObjectTrigger obj = other.gameObject.GetComponent<ObjectTrigger>();
            gm.StartDialogue(obj.dialogue);

            if (obj.isKey)
            {
                obj.door.SetActive(false);
                obj.doorDialogue.SetActive(false);
                other.gameObject.SetActive(false);
            }

        }
    }
}
