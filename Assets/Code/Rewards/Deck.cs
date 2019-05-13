using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public Character character;
    public DeckCard prefab;
    public List<DeckCard> cards;
    public int row;
    public int col;
    public Vector2 size;
    float width;
    float height;

    public GameObject panel;
    public Vector3 start;

    public bool activated;

    public void Start()
    {
        cards = new List<DeckCard>();
        size = panel.GetComponent<RectTransform>().sizeDelta;
        width = (size.x - 200) / col;
        height = (size.y) / row;
        activated = false;
        panel.SetActive(false);
    }

    public void SetCharacter(Character s)
    {
        character = s;
    }

    public void DisplayDeck()
    {
        if (activated)
        {
            foreach(DeckCard c in cards)
            {
                Destroy(c.gameObject);
            }
            cards.Clear();
        }
        else
        {
            for (int i = 0; i < character.stats.cards.Count; ++i)
            {
                int x = i % row;
                int y = i / row;
                Vector3 destination = new Vector3(start.x + x * width, start.y - y * height);

                DeckCard d = Instantiate(prefab);
                d.transform.SetParent(panel.transform);
                d.SetCard(character.stats.cards[i], character, destination, i);
                cards.Add(d);
            }
        }
        activated = !activated;
        panel.SetActive(activated);

    }
}
