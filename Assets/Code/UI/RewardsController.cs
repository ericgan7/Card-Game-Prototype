using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    public float rewardsCount;
    public Vector3 center;

    public List<CardMovement> hand;

    float width;
    public float selectionHeight;

    public List<Character> allies;
    public int allyIndex = 0;

    public RewardsUIController UIController;
    // Start is called before the first frame update
    void Start()
    {
        UIController.SelectCharacter(allies[allyIndex]);
    }

    public void DrawCurrentRewards()
    {
        List<Card> cards = allies[allyIndex].hand;
        int half = cards.Count / 2;
        float xSeperation = Mathf.Min(width, rewardsCount / half);
        Vector3 leftStart = new Vector3(center.x - xSeperation / 2, center.y - 1);
        Vector3 rightStart = new Vector3(center.x + xSeperation / 2, center.y - 1);
        int right = half;
        if (cards.Count % 2 == 1)
        {
            leftStart.x -= xSeperation / 2;
            rightStart.x += xSeperation / 2;
            hand[right].SetPosition(center);
            hand[right].UpdateCard(cards[right]);
            ++right;
        }
        for (int i = 0; i < half; ++i)
        {
            hand[i + right].SetPosition(new Vector3(rightStart.x + xSeperation * i, rightStart.y - i, -i));
            hand[i + right].UpdateCard(cards[i + right]);
        }
        for (int i = 0; i < half; ++i)
        {
            hand[half - i - 1].SetPosition(new Vector3(leftStart.x - xSeperation * i, rightStart.y - i, i));
            hand[half - i - 1].UpdateCard(cards[half - i - 1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
