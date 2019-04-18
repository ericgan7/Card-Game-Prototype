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
        StartGame();
        inputControl = GetComponent<InputController>();
        hand = FindObjectOfType<CardController>();
    }

    // Used to start game. Potentially can run an intro before calling this function
    public void StartGame()
    {
        // Temporary turn setup
        turns = new Queue<Character>(allies);
        ui.UpdateTurns(turns.ToList());
        currentCharacter = turns.Peek();
        hand.DrawCurrentCards(currentCharacter);
        ui.SelectCharacter(currentCharacter);
    }

    // Used to move character from mouse input. Called from InputController
    public void MoveCharacter(Vector3 position)
    {
        currentCharacter.Move(map.FindPath(map.WorldToCellSpace(currentCharacter.transform.position), map.WorldToCellSpace(position)));
        // Currently a player's turn ends once they move
        var c = turns.Dequeue();
        // Add the character back into the queue once they moved
        turns.Enqueue(c);
        currentCharacter = turns.Peek();
        ui.UpdateTurns(turns.ToList());
    }

    //Used to select a tile from mouse input. Called from InputController.
        //returns true if it is the current character, currently used to deteremine if play selects the current character's turn to enable movement.
    public bool SelectLocation(Vector3 position)
    {
        //check for current character & make tiles brighter?
        Character target = map.GetCharacter(map.WorldToCellSpace(position));
        if (target != null)
        {
            map.Highlight(target.transform.position, target.GetSpeed(), HighlightTiles.TileType.Move);
        }
        Debug.Log(target);
        return target == currentCharacter;
    }

    //  Highlights the tile map to indicate possible attack targets
    public void HighlightTargets(int area,  HighlightTiles.TileType t)
    {
        map.Highlight(currentCharacter.transform.position, area, t);
    }
    //  clears out tilemap for targeting.
    public void UnHiglightTarget(int area)
    {
        map.UnHighlight(currentCharacter.transform.position);
    }
    
    //Used to cast a card. To be implemented.
    public void Cast(Vector3 position)
    {

    }
}
