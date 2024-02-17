using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

[CreateAssetMenu(fileName = "dialgoueLine", menuName = "Dialgoue System/Create Line")]
public class DialogueLine : ScriptableObject
{
   public string dialogue;
   public CharacterID speaker;
   public int karmaScore;
   public AudioClip audio;
   public DialogueLine[] responses;
}
