using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "dialogueLine", menuName = "Dialogue System/Create Line")]
public class DialogueLine : ScriptableObject
{
    public string dialogue;
    public CharacterID speaker;
    public int karmaScore;

    public AudioClip audio;
    public DialogueLine[] responses;
}
