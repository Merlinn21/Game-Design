using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Character/Create new Character")]
public class Character : ScriptableObject
{
    [SerializeField] private string fullName;
    [SerializeField] private Sprite potrait;

    public string getFullName() { return fullName; }
    public Sprite getPotrait() { return potrait; }
}
