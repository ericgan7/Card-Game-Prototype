using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public CharacterSelectManager cm;
    public CharacterStats character;
    public Vector3 destination;
    public float speed;
    public int index;

    public Image image;
    public Text names;

    public void Start()
    {
        cm = FindObjectOfType<CharacterSelectManager>();
        destination = transform.position;
    }

    public void SetCharacter(CharacterStats c)
    {
        character = c;
        image.sprite = c.portrait;
        names.text = c.characterName;
    }

    public void Update()
    {
        transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
    }
    public void OnDrag(PointerEventData p)
    {
        destination = p.position;
        cm.CheckSwap(index);
    }

    public void OnEndDrag(PointerEventData p)
    {
        destination = cm.GetPosition(index);
    }
}
