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
        if(self.team == Card.TargetType.Ally)
        {
            Debug.Log("ENEMY AI has ally character");
            return;
        }
        maxrange = GetRange();
        IEnumerator coroutine = TakeActions();
        StartCoroutine(coroutine);
    }

    IEnumerator TakeActions()
    {
        for (int i = 0; i <= self.GetEnergy().x; ++i)
        {
            yield return new WaitForSeconds(1.0f);
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
                if (PlayBestCard(2))
                {
                    yield return new WaitForSeconds(2.0f);
                }
                else if (MakeMove()) { }
                else
                {
                    PlayBestCard(0);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }
        Debug.Log("Remaining energy:" + self.GetEnergy().x);
        yield return new WaitForSeconds(1.0f);
        game.UpdateTurn(new List<Card>());
    }
    
    public bool MakeMove()
    {
        if (self.GetSpeed() <= 0)
        {
            return false;
        }
        Debug.Log("Movement " + self.GetSpeed().ToString());
        game.map.move.UpdateMovementMap(self, maxrange - 1);
        Vector3Int current = game.map.WorldToCellSpace(self.transform.position);
        List<Vector3Int> path = new List<Vector3Int>();
        for (int i =0; i < self.GetSpeed(); ++i)
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
        path.Reverse();
        self.AddPath(path);
        self.Move();
        return true;
    }

    public bool PlayBestCard(int threshold)
    {
        List<CardScore> scores = new List<CardScore>();
        List<Character> allTargets = new List<Character>();
        allTargets = game.map.GetCharacterInRange(self.GetPosition(), maxrange);
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
        Debug.Log(max);
        if (max > threshold)
        {
            self.hand[index].Play(self, new List<Character> { scores[index].target });
            game.hand.PlayCard(self.hand[index], self);
            self.ChangeEnergy(-self.hand[index].energyCost);
            self.hand.RemoveAt(index);
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
