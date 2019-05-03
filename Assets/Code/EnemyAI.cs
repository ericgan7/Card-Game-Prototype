using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Character self;
    public GameController game;

    public void Action()
    {
        StartCoroutine("MakeMove");
    }
    
    IEnumerator MakeMove()
    {
        game.map.move.UpdateMovementMap(0);
        Vector3Int current = game.map.WorldToCellSpace(self.transform.position);
        yield return new WaitForSeconds(0.5f);
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
        path.Reverse();
        self.AddPath(path);
        self.Move();
    }

    public int bestPlayableCard()
    {
        foreach(Card c in self.hand)
        {
            
        }
        return 0;
    }
}
