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
    public float selectionHeight;
    public List<CardMovement> keep;

    //Temporary 
    Vector3 discardLocation = new Vector3(-354f, 90f);

    public void Start()
    {
        center = new Vector3(0f, 65f);
        maxHandLength = 150f;
        width = hand[0].GetComponent<RectTransform>().sizeDelta.x;
        keep = new List<CardMovement>();
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
            hand[right].SetPosition(center);
            hand[right].UpdateCard(cards[right]);
            ++right;
        }
        for (int i = 0; i < half; ++i)
        {
            hand[i + right].SetPosition(new Vector3(rightStart.x + xSeperation * i, rightStart.y - i, -i));
            hand[i + right].UpdateCard(cards[i+right]);
        }
        for (int i = 0; i < half; ++i)
        {
            hand[half - i - 1].SetPosition(new Vector3(leftStart.x - xSeperation * i, rightStart.y - i, i));
            hand[half - i - 1].UpdateCard(cards[half -i - 1]);
        }
    }

    public void DiscardDeck()
    {

    }

    public void DrawNewCard(Character deck)
    {

    }
    //remember to set isCardDrawn to false when discarding cards.
    public void DiscardCard()
    {

    }

    public void StartSelectionToKeep()
    {
        keep.Clear();
        foreach(CardMovement c in hand)
        {
            if (c.isCardDrawn)
            {
                c.SetPosition(new Vector3(c.transform.localPosition.x, transform.localPosition.y + selectionHeight));
                c.keepingMode = true;
            }
        }
    }

    public void SelectCardToKeep(CardMovement card)
    {
        if (keep.Contains(card))
        {
            keep.Remove(card);
            card.SetPosition(new Vector3(card.transform.localPosition.x, card.transform.localPosition.y + selectionHeight));
        }
        else
        {
            keep.Add(card);
            card.SetPosition(new Vector3(card.transform.localPosition.x, card.transform.localPosition.y - selectionHeight));
        }
    }

    public void FinishCardToKeepSelection()
    {
        List<Card> kept = new List<Card>();
        foreach (CardMovement c in keep)
        {
            kept.Add(c.cardData);
        }
        foreach(CardMovement c in hand)
        {
            c.keepingMode = false;
            c.isCardDrawn = false;
        }
        //game.EndAllyTurn(kept);
        game.UpdateTurn(kept);
    }
}
