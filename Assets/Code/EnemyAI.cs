using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Character self;
    public GameController game;
    
    public void MakeMove()
    {
        for (int i = 0; i < self.GetEnergy()[0]; ++i)
        {

        }
    }

    public int bestPlayableCard()
    {
        foreach(Card c in self.hand)
        {
            
        }
        return 0;
    }
}
