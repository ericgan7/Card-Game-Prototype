using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to hold information about a character, including current deck and starting stats.
/// </summary>

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterStats : ScriptableObject
{
    public Sprite portrait;
    public string characterName;
    public int health;
    public int energy;
    public int speed;
    public int evasion;
    public int armor;

    public List<Card> cards;

    public void AddCard(string cardName) { }
}
