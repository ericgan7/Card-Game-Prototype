using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to hold Card data. Should contain all behavior, strings, art, cost, etc.
/// </summary>

[CreateAssetMenu(fileName = "New Card", menuName = "Cards")]
public class Card : ScriptableObject
{
    public new string name;
    public string description;
    public Sprite art;
    public int energyCost;
    public Effect[] effects;
    public enum EffectType
    {
        Single, Area, Chain
    }
    public enum TargetType
    {
        Self, Ally, Enemy
    }
    public EffectType etype;
    public List<TargetType> targetsTypes;

    public int targetRange;
    public int effectRange;

    public void Play(List<Character> targets)
    {
        
    }
}
