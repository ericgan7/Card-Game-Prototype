using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    int startingHand = 5;
    GameController game;
    bool isMoving;
    public bool hasMoved;

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
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = stats.portrait;
        var bounds = sr.sprite.bounds;
        float factor = 1.3f / bounds.size.y;
        transform.localScale = new Vector3(factor, factor, factor);
        deck = new List<Card>(stats.cards);
        hand = new List<Card>();
        RefillHand(new List<Card>());
        isMoving = false;
        hasMoved = false;
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
            foreach (Card c in hand)
            {
                deck.Remove(c);
            }
        }
        return drawn;
    }

    // Keeps all specified cards and redraws discarded cards.
    public void RefillHand(List<Card> keeps)
    {
        hand.Clear();
        hand.AddRange(keeps);
        for (int i = 0; i < startingHand - keeps.Count(); ++i)
        {
            hand.Add(DrawRandom());
        }
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
        return currentSpeed - destinations.Count;
    }

    public int GetDamage()
    {
        return currentDamage;
    }
    public int GetArmor()
    {
        return currentArmor;
    }
    public bool HasEnergy()
    {
        return currentEnergy > 0;
    }

    //Settors
    public void ChangeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            //call game for death
        }
        Debug.Log("New Health " + currentHealth.ToString());
    }
    public void ChangeArmor(int amount)
    {
        currentArmor += amount;
        Debug.Log("New Armor " + currentArmor.ToString());
    }
   
    public void ChangeEnergy(int amount)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + amount, 0, stats.energy);
        if (currentEnergy <= 0)
        {
            game.EndAllyTurn(new List<Card>());
        }
        Debug.Log("New Energy " + currentEnergy.ToString());
    }

    //Update function controls movement of character across grid.
    public void FixedUpdate()
    {
        if (isMoving)
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
            else
            {
                isMoving = false;
                game.inputControl.disableInput = false;
                if (currentEnergy == 0)
                {
                    game.EndAllyTurn(new List<Card>());
                }
            }
        }
    }
    //set movement path, found during pathfinding.
    public void AddPath(List<Vector3Int> d)
    {
        d.AddRange(destinations);
        destinations = d;
        elapsed = 0f;
    }
    public void ClearPath()
    {
        destinations.Clear();
    }
    public void Move()
    {
        if (!hasMoved)
        {
            isMoving = true;
            game.inputControl.disableInput = true;
            hasMoved = true;
            ChangeEnergy(-1);
        }
    }
    //Reset Energy, trigger end of turn effects, etc.
    public void EndTurn()
    {
        hasMoved = false;
        currentEnergy = stats.energy;
    }
}
