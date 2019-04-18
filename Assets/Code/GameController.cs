using System.Collections;
using System.Collections.Generic;
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

    Vector3Int selectedMovementLocation;
    public Character currentCharacter;

    private void Start()
    {
        currentCharacter = allies[0];
        inputControl = GetComponent<InputController>();
        hand = FindObjectOfType<CardController>();
        StartGame();
    }

    // Used to start game. Potentially can run an intro before calling this function
    public void StartGame()
    {
        hand.DrawCurrentCards(currentCharacter);
        ui.SelectCharacter(currentCharacter);
        ui.UpdateTurns(new List<Character>(allies));
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

    //Used to select a tile from mouse input. Called from InputController.
        //returns true if it is the current character, currently used to deteremine if play selects the current character's turn to enable movement.
    public bool SelectLocation(Vector3 position)
    {
        //check for current character & make tiles brighter?
        Character target = map.GetCharacter(map.WorldToCellSpace(position));
        if (target != null)
        {
            map.Highlight(target.transform.position, target.GetSpeed(), HighlightTiles.TileType.Move, new List<Card.TargetType> { Card.TargetType.Ally, Card.TargetType.Enemy });
        }
        Debug.Log(target);
        return target == currentCharacter;
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
    
    //Used to cast a card. To be implemented.
    public void Cast(Vector3 position)
    {

    }
}
