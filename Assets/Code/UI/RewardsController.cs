using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsController : MonoBehaviour
{
    public int rewardsCount;
    public Vector3 center;
    
    public List<CardMovement> rewards;
    public Dictionary<Character, List<Card>> rewardMap = new Dictionary<Character, List<Card>>();

    float width;

    public List<Character> allies;
    public int allyIndex = 0;

    public RewardsUIController UIController;
    // Start is called before the first frame update
    void Start()
    {
        PopulateRewardMap();
        UIController.SelectCharacter(allies[allyIndex]);
        DrawCurrentRewards();
    }

    // Generates the rewards for each character
    public void PopulateRewardMap()
    {
        allies.ForEach(x =>
        {
            var rand = Random.Range(0, 2);
            rewardMap.Add(x, Enumerable.Repeat(rewards[rand].cardData, rewardsCount).ToList());
        });
    }

    public void DrawCurrentRewards()
    {
        List<Card> cards = rewardMap[allies[allyIndex]];
        int half = cards.Count / 2;
        float xSeperation = Mathf.Min(width, rewardsCount / half);
        Vector3 leftStart = new Vector3(center.x - xSeperation / 2, center.y - 1);
        Vector3 rightStart = new Vector3(center.x + xSeperation / 2, center.y - 1);
        int right = half;
        if (cards.Count % 2 == 1)
        {
            leftStart.x -= xSeperation / 2;
            rightStart.x += xSeperation / 2;
            rewards[right].SetPosition(center);
            rewards[right].UpdateCard(cards[right]);
            ++right;
        }
        for (int i = 0; i < half; ++i)
        {
            rewards[i + right].SetPosition(new Vector3(rightStart.x + xSeperation * i, rightStart.y - i, -i));
            rewards[i + right].UpdateCard(cards[i + right]);
        }
        for (int i = 0; i < half; ++i)
        {
            rewards[half - i - 1].SetPosition(new Vector3(leftStart.x - xSeperation * i, rightStart.y - i, i));
            rewards[half - i - 1].UpdateCard(cards[half - i - 1]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
