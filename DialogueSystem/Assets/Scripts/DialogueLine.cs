using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string GUID;
    public Vector2 position;

    public string dialogue;
    public CharacterID speaker;
    public int karmaScore;

    public AudioClip audio;

    [System.NonSerialized]
    public DialogueLine[] responses;
}

[Serializable]
public class NodeLinkData
{
    public string BaseNodeGUID;
    public string PortName;
    public string TargetNodeGUID;
}
