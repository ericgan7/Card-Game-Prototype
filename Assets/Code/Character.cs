using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to control the character on the grid and keeps track of current stats, which may be effected by buffs or debuffs.
///     also keep track of current hand
/// </summary>

public class Character : MonoBehaviour
{
    public List<Vector3Int> destinations;
    public CharacterStats stats;
    public Card.TargetType team;
    int currentHealth;
    int currentEnergy;
    int currentSpeed;
    int currentArmor;
    int currentDamage;
    public List<Effect> statusEffects;

    List<Card> deck;
    public List<Card> hand;

    Vector3 startLocation;
    public float movementSpeed;
    float elapsed;
    Vector3 offset;

    int startingHand = 3;
    GameController game;

    private void Start()
    {
        game = FindObjectOfType<GameController>();
        destinations = new List<Vector3Int>();
        startLocation = transform.localPosition;
        offset = new Vector3(0.5f, 0.5f, 0f);
        currentHealth = stats.health;
        currentEnergy = stats.energy;
        currentSpeed = stats.speed;
        currentDamage = stats.damage;
        currentArmor = stats.armor;
        statusEffects = new List<Effect>();
        deck = new List<Card>(stats.cards);
        hand = new List<Card>();
        for (int i = 0; i < startingHand; ++i)
        {
            hand.Add(DrawRandom());
        }
    }
    //draw a random card from deck. untill all cards are drawn. They are then replenished.
    public Card DrawRandom()
    {
        int index = Random.Range(0, deck.Count);
        Card drawn = deck[index];
        deck.Remove(drawn);
        if (deck.Count == 0)
        {
            deck.AddRange(stats.cards);
        }
        return drawn;
    }
    //Gettors
    public Vector2Int GetHealth()
    {
        return new Vector2Int(currentHealth, stats.health);
    }

    public Vector2Int GetEnergy()
    {
        return new Vector2Int(currentEnergy, stats.energy);
    }
    
    public int GetSpeed()
    {
        return currentSpeed;
    }

    public int GetDamage()
    {
        return currentDamage;
    }
    public int GetArmor()
    {
        return currentArmor;
    }
    //Settors
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            //call game for death
        }
    }
    public void ChangeArmor(int amount)
    {
        currentArmor += amount;
    }

    //Update function controls movement of character across grid.
    public void FixedUpdate()
    {
        if (destinations.Count > 0)
        { 
            if (Vector3.Distance(transform.localPosition, destinations[destinations.Count - 1]) < 0.01f)
            {
                game.map.UpdateCharacterMap(this, destinations[destinations.Count - 1]);
                destinations.RemoveAt(destinations.Count - 1);
                startLocation = transform.localPosition;
                elapsed = 0f;
            }
            else
            {
                elapsed += Time.deltaTime;
                transform.localPosition = Vector3.Lerp(startLocation, destinations[destinations.Count - 1], elapsed * movementSpeed);
            }
        }
    }
    //set movement path, found during pathfinding.
    public void Move(List<Vector3Int> d)
    {
        destinations = d;
        elapsed = 0f;
    }
}
