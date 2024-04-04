using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "characterID", menuName = "Dialogue System/Create Character")]
public class CharacterID : ScriptableObject
{
    public string characterName;
    public Texture2D image;
}
