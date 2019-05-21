using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;

/// <summary>
/// Used to hold information about a character, including current deck and starting stats.
/// </summary>

[CreateAssetMenu(fileName = "New Character", menuName = "Character")]
public class CharacterStats : ScriptableObject
{
    public Sprite portrait;
    public Sprite idle;
    public List<string> spriteNames;
    public Sprite[] sprites;
    public string characterName;
    public int health;
    public int energy;
    public int speed;
    public int armor;
    public int damage;

    public List<Card> cards;
    public List<Card.TargetType> moveableTiles;

    public RewardPool reward;

    public void AddCard(Reward r)
    {
        Debug.Log("add reward");
        if (r.GetType() == typeof(StatReward))
        {
            Debug.Log("stats");
            StatReward c = (StatReward)r;
            health += c.healthBonus;
            energy += c.energyBonus;
            speed += c.speedBonus;
            armor += c.armorBonus;
            damage += c.damageBonus;
        }
        else
        {
            Card c = (Card)r;
            cards.Add(c);
        }
    }

    public Sprite GetSprite(string name)
    {
        return sprites[spriteNames.FindIndex(x => x == name)];
    }
}



