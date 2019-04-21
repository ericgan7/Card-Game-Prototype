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
    int enemyIndex;
    int allyIndex;
    public Queue<Character> turns;

    Vector3Int selectedMovementLocation;
    public Character currentCharacter;

    private void Start()
    {
        inputControl = GetComponent<InputController>();
        hand = FindObjectOfType<CardController>();
        turns = new Queue<Character>();
        //9 should be replaced by the number of character turn displays.
        for (int i = 0; i < 9; ++i)
        {
            turns.Enqueue(allies[i % allies.Length]);
            turns.Enqueue(enemies[i % enemies.Length]);
            enemyIndex = (i + 1) % enemies.Length;
            allyIndex = (i + 1) % allies.Length;
        }
        StartGame();
    }

    // Used to start game. Potentially can run an intro before calling this function
    public void StartGame()
    {
        currentCharacter = turns.Peek();
        ui.UpdateTurns(turns.ToList());
        ui.SelectCharacter(currentCharacter);
        hand.DrawCurrentCards(currentCharacter);
    }

    //  Highlights the tile map to indicate possible attack targets
    public void HighlightTargets(int area, HighlightTiles.TileType t, List<Card.TargetType> validTargets)
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

    public void EndAllyTurn(List<Card> keep)
    {
        //Finish Current Character's Turn
        currentCharacter.RefillHand(keep);
        currentCharacter.RefillHand(keep);
        currentCharacter.EndTurn();
        UpdateTurn();
        turns.Enqueue(allies[allyIndex]);
        allyIndex = (allyIndex + 1) % allies.Length;
        //Enemy Action Turn
        EnemyTurn();
    }

    public void UpdateTurn()
    {
        //TODO check if there is only one character left in a team.
        turns.Dequeue();
        currentCharacter = turns.Peek();
        ui.UpdateTurns(turns.ToList());
    }
    //TODO AI action
    public void EnemyTurn()
    {
        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        UpdateTurn();
        turns.Enqueue(enemies[enemyIndex]);
        enemyIndex = (enemyIndex + 1) % enemies.Length;

        //Begin Next Character's Turn
        hand.DrawCurrentCards(currentCharacter);
    }
}
