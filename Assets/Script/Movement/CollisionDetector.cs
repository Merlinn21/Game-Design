using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private bool isWall = false;

    public bool getWall() { return isWall; }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Object") || other.CompareTag("Door") || other.CompareTag("Jalangkung") || other.CompareTag("Stair"))
        {
            isWall = true;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Object") || other.CompareTag("Door") || other.CompareTag("Jalangkung") || other.CompareTag("Stair"))
        {
            isWall = true;
            Debug.Log(other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Wall") || other.CompareTag("Object") || other.CompareTag("Door") || other.CompareTag("Jalangkung") || other.CompareTag("Stair"))
        {
            isWall = false;
        }
    }
}
