using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public MapController map;
    public InputController inputControl;
    public UIController ui;
    public CardController hand;

    public List<Character> enemies;
    public List<Character> allies;
    int enemyIndex;
    int allyIndex;
    public List<Character> turns;

    Vector3Int selectedMovementLocation;
    public Character currentCharacter;

    private void Start()
    {
        inputControl = GetComponent<InputController>();
        hand = FindObjectOfType<CardController>();
        turns = new List<Character>();
        PopulateTurns(0,0, true);
        StartGame();
    }

    // Used to start game. Potentially can run an intro before calling this function
    public void StartGame()
    {
        currentCharacter = turns.First();
        ui.UpdateTurns(turns);
        ui.SelectCharacter(currentCharacter);
        hand.DrawCurrentCards(currentCharacter);
    }

    public void PopulateTurns(int ally, int enemy, bool allyIsNext)
    {
        turns.Clear();
        int a = ally;
        int e = enemy;
        bool team = allyIsNext;
        while (turns.Count < 9)
        {
            if (team)
            {
                Debug.Log(a);
                turns.Add(allies[a]);
                a = (a + 1) % allies.Count;
            }
            else
            {
                Debug.Log(e);
                turns.Add(enemies[e]);
                e = (e + 1) % enemies.Count;
            }
            team = !team;
        }
        allyIndex = a;
        enemyIndex = e;
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
        Debug.Log("Cast Card");
        bool success = false;
        if (cardPlayed.targetsTypes.Contains(Card.TargetType.Ground))
        {
            // Allows Ground targeting for traps
        }
        else
        {
            List<Character> targets = map.targets.GetTargets();
            Debug.Log(targets.Count);
            if (targets.Count > 0 && currentCharacter.HasEnergy())
            {
                success = true;
                cardPlayed.Play(currentCharacter, targets);
                //Energy Cost will be deducted at the end of the card animation.
            }
        }
        return success;
    }

    public void EndAllyTurn(List<Card> keep)
    {
        //Finish Current Character's Turn
        currentCharacter.RefillHand(keep);
        currentCharacter.EndTurn();
        UpdateTurn();
        turns.Add(allies[allyIndex]);
        allyIndex = (allyIndex + 1) % allies.Count;
        //Enemy Action Turn
        EnemyTurn();
    }

    public void UpdateTurn()
    {
        //TODO check if there is only one character left in a team.
        turns.RemoveAt(0);
        currentCharacter = turns.First();
        ui.UpdateTurns(turns);
    }
    //TODO AI action
    public void EnemyTurn()
    {
        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        UpdateTurn();
        turns.Add(enemies[enemyIndex]);
        enemyIndex = (enemyIndex + 1) % enemies.Count;

        //Begin Next Character's Turn
        hand.DrawCurrentCards(currentCharacter);
    }

    public void KillAlly(Character character)
    {
        Debug.Log("An Ally has been slained");
    }

    public void KillEnemy(Character character)
    {
        Debug.Log("An Enemy has been slained");
        enemies.Remove(character);
        turns.RemoveAll(x => x == character);
        ui.UpdateTurns(turns);
    }
}
