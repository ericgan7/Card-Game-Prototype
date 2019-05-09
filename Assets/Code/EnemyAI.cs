using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Character self;
    public GameController game;
    int maxrange;
    public struct CardScore
    {
        public int score;
        public Character target;
    }

    public void Action()
    {
        maxrange = GetRange();
        IEnumerator coroutine = TakeActions();
        StartCoroutine(coroutine);
    }

    IEnumerator TakeActions()
    {
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < self.GetEnergy().x; ++i)
        {
            if  (self.GetEnergy().x <= 0)
            {
                break;
            }
            else
            {
                while(self.destinations.Count > 0)
                {
                    yield return new WaitForSeconds(0.5f);
                }
                if (!PlayBestCard())
                {
                    MakeMove();
                }
            }
        }
        game.UpdateTurn(new List<Card>());
    }
    
    public void MakeMove()
    {
        game.map.move.UpdateMovementMap(self, maxrange - 1);
        Vector3Int current = game.map.WorldToCellSpace(self.transform.position);
        List<Vector3Int> path = new List<Vector3Int>();
        for (int i =0; i < 3; ++i)
        {
            Vector2Int next = game.map.move.NextMove(current);
            if (next.x >= 0)
            {
                path.Add((Vector3Int)next);
                current = (Vector3Int)next;
            }
            else
            {
                Debug.Log("Optimal movement distance");
                break;
            }
        }
        Debug.Log("Movement");
        path.Reverse();
        self.AddPath(path);
        self.Move();
    }

    public bool PlayBestCard()
    {
        List<CardScore> scores = new List<CardScore>();
        List<Character> allTargets = new List<Character>();
        allTargets = game.map.GetCharacterInRange(self.GetPosition(), maxrange);
        Debug.Log(allTargets.Count);
        foreach(Card c in self.hand)
        {
            scores.Add(c.GetScore(self, allTargets));
        }
        int index = 0;
        int max = 0;
        for (int i = 0; i < scores.Count; ++i)
        {
            if (scores[i].score > max)
            {
                max = scores[i].score;
                index = i;
            }
        }
        //card playing threshhold
        if (max > 0)
        {
            self.hand[index].Play(self, new List<Character> { scores[index].target });
            self.ChangeEnergy(-self.hand[index].energyCost);
            Debug.Log(self.hand[index].name);
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetRange()
    {
        int range = 0;
        foreach (Card c in self.hand)
        {
            range = Mathf.Max(range, c.targetRange);
        }
        return range;
    }
}
