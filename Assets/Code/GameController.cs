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
    public Character[] obstacles;
    Queue<Character> allyTurn;
    Queue<Character> enemyTurn;
    public List<Character> turns;

    Vector3Int selectedMovementLocation;
    public Character currentCharacter;

    EnemyAI ai;
    bool switchingTurn;

    private void Start()
    {
        inputControl = GetComponent<InputController>();
        hand = FindObjectOfType<CardController>();
        turns = new List<Character>();
        allyTurn = new Queue<Character>(allies);
        enemyTurn = new Queue<Character>(enemies);
        StartGame();
        ai = GetComponent<EnemyAI>();
        ai.game = this;
        switchingTurn = false;
    }

    public void PopulateTurns(bool allyFirst)
    {
        turns.Clear();
        bool turn = allyFirst;
        Character temp;
        while(turns.Count < 10)
        {
            if (turn)
            {
                temp = allyTurn.Dequeue();
                turns.Add(temp);
                allyTurn.Enqueue(temp);
            }
            else
            {
                temp = enemyTurn.Dequeue();
                turns.Add(temp);
                enemyTurn.Enqueue(temp);
            }
            turn = !turn;
        }
    }

    // Used to start game. Potentially can run an intro before calling this function
    public void StartGame()
    {
        PopulateTurns(true);
        currentCharacter = turns[0];
        ui.UpdateTurns(turns.ToList());
        ui.SelectCharacter(currentCharacter);
        hand.DrawCurrentCards(currentCharacter);
    }

    //  Highlights the tile map to indicate possible attack targets
    public void HighlightTargets(Card.RangeType rt, int area, HighlightTiles.TileType t, List<Card.TargetType> validTargets)
    {
        map.Highlight(currentCharacter.transform.position, rt, area, t, validTargets);
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
        if (currentCharacter.GetEnergy().x < cardPlayed.energyCost)
        {
            return false;
        }
        if (cardPlayed.targetsTypes.Contains(Card.TargetType.Ground))
        {
            List<Vector3Int> tiles = map.targets.GetTiles();
            if (tiles.Count > 0)
            {
                success = true;
                cardPlayed.Play(currentCharacter, tiles);
            }
        }
        else
        {
            if (cardPlayed.etype == Card.EffectType.Targetable)
            {
                List<Character> targets = map.targets.GetTargets();
                if (targets.Count > 0)
                {
                    success = true;
                    cardPlayed.Play(currentCharacter, targets);
                    //Energy Cost will be deducted at the end of the card animation.
                }
            }
            else
            {
                List<Character> targets = map.highlights.GetTargets();
                if (targets.Count > 0 && currentCharacter.HasEnergy())
                {
                    success = true;
                    cardPlayed.Play(currentCharacter, targets);
                }
            }
        }
        if (success)
        {
            currentCharacter.ChangeEnergy(-cardPlayed.energyCost);
        }
        return success;
    }
    /* Treat enemis and allies the same for now, until enemy AI is done.
    public void EndAllyTurn(List<Card> keep)
    {
        //Finish Current Character's Turn
        currentCharacter.RefillHand(keep);
        currentCharacter.EndTurn();
        UpdateTurn(true);
        //Enemy Action Turn
        EnemyTurn(); 
    }
    */
    public void UpdateTurn(List<Card> keep)
    {
        //Temp;
        Debug.Log("NEW TURN");
        currentCharacter.RefillHand(keep);
        currentCharacter.EndTurn();
        //TODO check if there is only one character left in a team.
        if (currentCharacter.team == Card.TargetType.Ally)
        {
            Character c = allyTurn.Dequeue();
            turns.Add(c);
            allyTurn.Enqueue(c);
        }
        else
        {
            Character c = enemyTurn.Dequeue();
            turns.Add(c);
            enemyTurn.Enqueue(c);
        }
        turns.RemoveAt(0);
        IEnumerator coroutine = StartNextTurn();
        StartCoroutine(coroutine);
    }

    public IEnumerator StartNextTurn()
    {
        currentCharacter = turns[0];
        Vector3 p = map.WorldToCellSpace(currentCharacter.transform.position);
        p.z = Camera.main.transform.localPosition.z;
        inputControl.CenterCamera(p);

        ui.UpdateTurns(turns);
        currentCharacter.OnTurnStart();
        if (currentCharacter.GetHealth().x > 0)
        {
            if (currentCharacter.team == Card.TargetType.Enemy)
            {
                ai.self = currentCharacter;
                ai.Action();
            }
            else
            {
                hand.DrawCurrentCards(currentCharacter);
            }
            ui.SelectCharacter(currentCharacter);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            UpdateTurn(new List<Card>());
        }
    }
    //TODO AI action
    public void EnemyTurn()
    {
        EndEnemyTurn();
    }

    public void EndEnemyTurn()
    {

        //Begin Next Character's Turn
        hand.DrawCurrentCards(currentCharacter);
    }

    public void PlayAction()
    {
        //ui.PlayAction(results, currentCharacter.team == Card.TargetType.Ally);
    }

    public void KillCharacter(Character c, bool isAlly)
    {
        List<Character> turnq;
        if (isAlly)
        {
            turnq = allyTurn.ToList();
        }
        else
        {
            turnq = enemyTurn.ToList();
        }
        // Roll back queue status one turn so we can rebuild it.
            //The queue is cyclical, so order is maintained and the last element is the most recent character for turn ordering
        Character temp = turnq[turnq.Count - 1];
        turnq.Remove(temp);
        turnq.Insert(0, temp);
        //remove dead from queue
        turnq.Remove(c);
        if (isAlly)
        {
            allyTurn.Clear();
            //rebuild queue
            foreach (Character t in turnq)
            {
                allyTurn.Enqueue(t);
            }
            //restore order - might need to be changed if dead character was first - more testing needed;
            allyTurn.Enqueue(allyTurn.Dequeue());
        }
        //mirror for enemy
        else
        {
            enemyTurn.Clear();
            foreach (Character t in turnq)
            {
                enemyTurn.Enqueue(t);
            }
            enemyTurn.Enqueue(enemyTurn.Dequeue());
        }
        //rebuild turns by replacement
        int start = 1;
        if (currentCharacter.team == c.team)
        {
            start = 0;
        }
        int j = 0;
        for (int i = start; i < turns.Count; i += 2)
        {
            turns[i] = turnq[j];
            j = (j + 1) % turnq.Count;
        }
        map.RemoveCharacter(c);
        ui.UpdateTurns(turns);
    }

    public void GameOver()
    {
        GM progress = FindObjectOfType<GM>();
        progress.Next();
        progress.loadDialogue();
        gameover = false;

    }
    
    public bool gameover = false;

    public void Update()
    {
        if (gameover)
        {
            GameOver();
        }
    }

}
