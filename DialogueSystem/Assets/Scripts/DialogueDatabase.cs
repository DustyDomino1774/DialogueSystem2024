using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogueDatabase", menuName = "Dialogue System/Create Database")]
public class DialogueDatabase : ScriptableObject
{
    public List<DialogueLine> database = new List<DialogueLine>();
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
}
