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
    int naturalArmor;
    int currentDamage;
    public List<Effect> statusEffects;

    List<Card> deck;
    public List<Card> hand;

    Vector3 startLocation;
    public float movementSpeed;
    float elapsed;
    Vector3 offset;

    public int startingHand = 5;
    public GameController game;
    bool isMoving;
    public bool hasMoved;

    public HealthBarController hbc;
    public HealthBar healthBarPrefab;
    public HealthBar healthBar;

    public void Start()
    {
        game = FindObjectOfType<GameController>();
        destinations = new List<Vector3Int>();
        startLocation = transform.localPosition;
        offset = new Vector3(0.5f, 0.5f, 0f);
        transform.position = game.map.WorldToCellSpace(transform.position) + new Vector3(0.5f, 0.5f, 0f);
        SetStats();
        if (!(team == Card.TargetType.Obstacle || team == Card.TargetType.PassableObstacle))
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.sprite = stats.idle;
            var bounds = sr.sprite.bounds;
            float factor = 1.3f / bounds.size.y;
            transform.localScale = new Vector3(factor, factor, factor);
            deck = new List<Card>(stats.cards);
            hand = new List<Card>();
            RefillHand(new List<Card>());
            isMoving = false;
            hasMoved = false;
            healthBar = hbc.createHealthBar(this);
        }
    }

    public void SetStats()
    {
        currentHealth = stats.health;
        currentEnergy = stats.energy;
        currentSpeed = stats.speed;
        currentDamage = stats.damage;
        currentArmor = stats.armor;
        naturalArmor = currentArmor;
        statusEffects = new List<Effect>();
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

    // Draws cards from the deck from card effect
    public void DrawCards(int cardDraw)
    {
        for (var i = 0; i < cardDraw; ++i)
        {
            hand.Add(DrawRandom());
        }
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
    public Vector3Int GetPosition()
    {
        if (destinations.Count > 0)
        {
            return destinations[0];
        }
        else
        {
            return game.map.WorldToCellSpace(transform.position);
        }
    }

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
        int mod = 0;
        foreach (Effect e in statusEffects)
        {
            mod += e.ModifySpeed();
        }
        return currentSpeed - destinations.Count + mod;
    }

    public int GetDamage()
    {
        int mod = 0;
        foreach(Effect e in statusEffects)
        {
            mod += e.ModifyAttack();
        }
        return currentHealth + mod;
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
    public int ChangeHealth(int amount)
    {
        if (team == Card.TargetType.PassableObstacle)
        {
            return 0;
        }
        int temp = currentHealth;
        currentHealth = Mathf.Clamp(amount + currentHealth, 0, stats.health);
        if (currentHealth <= 0)
        {
            game.KillCharacter(this, team == Card.TargetType.Ally);
            gameObject.SetActive(false);
        }
        Debug.Log("New Health " + currentHealth.ToString());
        int change = currentHealth - temp;
        Vector3 p = transform.position;
        p.y += 1;
        p.x += Random.Range(-0.5f, 0.5f);
        Color c = Color.red;
        if (change > 0)
        {
            c = Color.green;
        }
        game.ui.displayText.CreateWorldText(transform.position, p, change.ToString(), c);
        return Mathf.Abs(change);
    }
    public int ChangeArmor(int amount, bool healArmor = false)
    {
        int temp = currentArmor;
        if (healArmor)
        {
            naturalArmor += amount;
        }
        currentArmor = Mathf.Clamp(currentArmor + amount, 0, 100);
        Debug.Log("New Armor " + currentArmor.ToString());
        int change = currentArmor - temp;
        Vector3 p = transform.position;
        p.y += 1;
        p.x += Random.Range(-0.5f, 0.5f);
        Color c = Color.blue;
        if (change > 0)
        {
            c = Color.cyan;
        }
        game.ui.displayText.CreateWorldText(transform.position, p, change.ToString(), c);
        return Mathf.Abs(change);
    }
   
    public int ChangeEnergy(int amount)
    {
        currentEnergy = currentEnergy + amount;
        Debug.Log("New Energy " + currentEnergy.ToString());
        return amount;
    }

    public void ChangeDamage(int amount)
    {
        currentDamage = amount;
    }

    public int ChangeSpeed(int amount)
    {
        currentSpeed = currentSpeed + amount;
        return amount;
    }

    public void AddStatusEffect(Effect newEffect, bool stack)
    {
        bool exists = false;
        if (stack)
        {
            foreach (Effect e in statusEffects)
            {
                if (e.GetType() == newEffect.GetType())
                {
                    exists = true;
                    e.StackEffect(newEffect);
                }
            }
        }
        if (!exists)
        {
            statusEffects.Add(newEffect);
        }
        string t = newEffect.GetAmount(this, this);
        Vector3 up = gameObject.transform.position + new Vector3(0f, 1f);
        game.ui.displayText.CreateWorldText(gameObject.transform.position, up, t, newEffect.color);
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
                    game.map.MoveCharacter(this, destinations[destinations.Count - 1]);
                    destinations.RemoveAt(destinations.Count - 1);
                    startLocation = transform.localPosition;
                    foreach(Effect e in statusEffects)
                    {
                        e.OnMove(this);
                    }
                    currentSpeed -= 1;
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
                AudioManager.instance.stopPlaying();
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
    public void Move(int cost = 1)
    {
        isMoving = true;
        game.inputControl.disableInput = true;
        hasMoved = true;
        ChangeEnergy(-cost);
        AudioManager.instance.playWalk();
    }
    //Reset Energy, trigger end of turn effects, etc.
    public void EndTurn()
    {
        currentEnergy = stats.energy;
        currentSpeed = stats.speed;
        healthBar.SetFrame(false);
    }

    public void OnTurnStart()
    {
        for (int i = 0; i < statusEffects.Count; ++i)
        {
            statusEffects[i].OnTurnStart(this);
        }
        /*
        //Lowers current armor if block is in effect
        currentArmor = Mathf.Min(naturalArmor, currentArmor); 
        //Lowers natural armor if has been damaged
        naturalArmor = Mathf.Min(naturalArmor, currentArmor);
        */
        isMoving = false;
        healthBar.SetFrame(true);
    }
}
