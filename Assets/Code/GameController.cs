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

    public void StartGame()
    {
        hand.DrawCurrentCards(currentCharacter);
        ui.SelectCharacter(currentCharacter);
        ui.UpdateTurns(new List<Character>(allies));
    }

    public void MoveCharacter(Vector3 position)
    {
        currentCharacter.Move(map.FindPath(map.WorldToCellSpace(currentCharacter.transform.position), map.WorldToCellSpace(position)));
    }

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

    public void HighlightTargets(int area,  HighlightTiles.TileType t)
    {
        map.Highlight(currentCharacter.transform.position, area, t);
    }
    public void UnHiglightTarget(int area)
    {
        map.UnHighlight(currentCharacter.transform.position);
    }
    
    public void Cast(Vector3 position)
    {

    }
}
