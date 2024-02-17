using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialgoueDatabase", menuName = "Dialgoue System/Create Database")]
public class DialogueDatabase : ScriptableObject
{
    public DialogueLine[] database;
}
