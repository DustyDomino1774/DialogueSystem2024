using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogueDatabase", menuName = "Dialogue System/Create Database")]
public class DialogueDatabase : ScriptableObject
{
    public DialogueLine[] database;
}
