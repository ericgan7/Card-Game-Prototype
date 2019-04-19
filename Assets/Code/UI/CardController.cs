using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gamecontroller is used to handle game logic and servers as an interface for all other classes.
/// </summary>

public class CardController : MonoBehaviour
{
    public GameController game;
    public List<CardMovement> hand;
    public Transform parent;
    public float maxHandLength;
    public Vector3 center;

    float width;

    public void Start()
    {
        center = new Vector3(0f, 65f);
        maxHandLength = 150f;
        width = hand[0].GetComponent<RectTransform>().sizeDelta.x;
    }

    public void DrawCurrentCards(Character character)
    {
        List<Card> cards = game.currentCharacter.hand;
        int half = cards.Count / 2;
        float xSeperation = Mathf.Min(width, maxHandLength / half);
        Vector3 leftStart = new Vector3(center.x - xSeperation / 2, center.y - 1);
        Vector3 rightStart = new Vector3(center.x + xSeperation / 2, center.y - 1);
        int right = half;
        if (cards.Count % 2 == 1)
        {
            leftStart.x -= xSeperation / 2;
            rightStart.x += xSeperation / 2;
            hand[right].SetPosition(parent.transform.TransformPoint(center));
            hand[right].UpdateCard(cards[right]);
            ++right;
        }
        for (int i = 0; i < half; ++i)
        {
            hand[i + right].SetPosition(parent.transform.TransformPoint(new Vector3(rightStart.x + xSeperation * i, rightStart.y - i, -i)));
            hand[i + right].UpdateCard(cards[i]);
        }
        for (int i = 0; i < half; ++i)
        {
            hand[half - i - 1].SetPosition(parent.transform.TransformPoint(new Vector3(leftStart.x - xSeperation * i, rightStart.y - i, i)));
            hand[half - i - 1].UpdateCard(cards[i]);
        }
    }

    public void DiscardDeck()
    {

    }

    public void DrawNewCard(Character deck)
    {

    }

    public void DiscardCard()
    {

    }
}
