using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public MapController map;
    public InputController inputControl;
    public UIController ui;
    public CardController hand;

    public Character[] enemies;
    public Character[] allies;
    public Queue<Character> turns;

    Vector3Int selectedMovementLocation;
    public Character currentCharacter;
    private void Start()
    {
        inputControl = GetComponent<InputController>();
        hand = FindObjectOfType<CardController>();
        turns = new Queue<Character>(allies);
        StartGame();
    }

    // Used to start game. Potentially can run an intro before calling this function
    public void StartGame()
    {
        // Temporary turn setup
        UpdateTurn();
        ui.UpdateTurns(turns.ToList());
        ui.SelectCharacter(currentCharacter);
    }

    // Used to move character from mouse input. Called from InputController
    public void MoveCharacter(Vector3 position)
    {
        Vector3Int location = map.WorldToCellSpace(position);
        if (map.highlights.tilesFilled.ContainsKey(location))
        {
            Debug.Log("attempt move");
            currentCharacter.Move(map.FindPath(map.WorldToCellSpace(currentCharacter.transform.position), location));
        }
    }

    //  Highlights the tile map to indicate possible attack targets
    public void HighlightTargets(int area,  HighlightTiles.TileType t, List<Card.TargetType> validTargets)
    {
        map.Highlight(currentCharacter.transform.position, area, t, validTargets);
    }
    //  clears out tilemap for targeting.
    public void UnHiglightTarget(int area)
    {
        map.UnHighlight(currentCharacter.transform.position);
    }
    
    //Used to cast a card. Targets on Grid are used to determine tiles effected, set during ondrag();
    public bool Cast(Card cardPlayed)
    {
        bool success = false;
        if (cardPlayed.targetsTypes.Contains(Card.TargetType.Ground))
        {
            // Allows Ground targeting for traps
        }
        else
        {
            List<Character> targets = map.targets.GetTargets();
            Debug.Log(targets.Count);
            if (targets.Count > 0)
            {
                success = true;
                cardPlayed.Play(currentCharacter, targets);
            }
        }
        return success;
    }

    public void UpdateTurn()
    {
        // Currently a player's turn ends once they move
        var c = turns.Dequeue();
        // Add the character back into the queue once they moved
        turns.Enqueue(c);
        currentCharacter = turns.Peek();
        // Deal new hand to new player
        currentCharacter.RefillHand(new List<int>());
        ui.UpdateTurns(turns.ToList());
        hand.DrawCurrentCards(currentCharacter);
    }
}
