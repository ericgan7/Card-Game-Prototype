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
    Vector2 center;
    Vector2 size;

    public float xSeperation;
    public float ySeperation;

    public void Start()
    {
        center = new Vector2(0f, 65f);
        size = new Vector2(75f, 105f);
    }

    public void DrawCurrentCards(Character character)
    {
        List<Card> cards = game.currentCharacter.hand;
        Vector2 start = new Vector2((-cards.Count / 2) * xSeperation, 65f - cards.Count / 2 * ySeperation);
        for (int i = 0; i < cards.Count / 2; ++i)
        {
            hand[i].SetPosition(parent.transform.TransformPoint(new Vector3(start.x + xSeperation * i, start.y + ySeperation * i)));
        }
        if (cards.Count % 2 == 1)
        {
            int index = cards.Count / 2;
            hand[index].SetPosition(parent.transform.TransformPoint(new Vector3(center.x, center.y)));
        }
        for (int i = cards.Count / 2 + 1; i < cards.Count; ++i)
        {
            hand[i].SetPosition(parent.transform.TransformPoint(new Vector3(start.x + xSeperation * i, start.y + ySeperation * -(i - cards.Count))));
        }
        for (int i = 0; i < cards.Count; ++i)
        {
            hand[i].UpdateCard(cards[i]);
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
