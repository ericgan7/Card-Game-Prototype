using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DeckCard : MonoBehaviour, IPointerClickHandler
{
    public bool selected;
    public Vector3 scale;
    public float speed;
    int index;
    public Vector3 defaultPosition;
    public Vector3 destination;

    public Image artwork;
    public TextMeshProUGUI cardname;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;

    public void Start()
    {
        selected = false;
        scale = Vector3.one;
    }

    public void SetCard(Card data, Character character, Vector3 end, int sibling)
    {
        artwork.sprite = data.art;
        cardname.text = data.name;
        description.text = data.GetDescription(character);
        cost.text = data.energyCost.ToString();
        defaultPosition = end;
        destination = end;
        index = sibling;
    }

    public void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * speed);
        transform.localPosition = Vector3.Lerp(transform.localPosition, destination, Time.deltaTime * speed);
    }

    public void OnPointerClick(PointerEventData p)
    {
        if (selected)
        {
            scale = Vector3.one;
            destination = defaultPosition;
            transform.SetSiblingIndex(index);
        }
        else
        {
            scale = new Vector3(3.0f, 3.0f, 3.0f);
            destination = Vector3.zero;
            transform.SetAsLastSibling();
        }
        selected = !selected;
    }
}
