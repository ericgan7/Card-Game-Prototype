using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class used for card handling. Can be given a Card to set the art and and effect when played.
/// </summary>

public class CardMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler
{
    public GameController game;
    public Card cardData;
    public Vector3 destination;
    Vector3 defaultLocation;
    public float speed;
    Rigidbody2D rb;
    TargetJoint2D tj;

    public Image artwork;
    public TextMeshProUGUI cardname;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;

    public void Start()
    {
        destination = transform.position;
        defaultLocation = transform.position;
        UpdateCard(cardData);
        rb = GetComponent<Rigidbody2D>();
        game = FindObjectOfType<GameController>();
        tj = GetComponent<TargetJoint2D>();
    }

    //Set position, used to draw from deck into hand.
    public void SetPosition(Vector3 newPostion)
    {
        defaultLocation = newPostion;
        tj.target = newPostion;
        tj.enabled = true;
        destination = newPostion;
        Debug.Log(newPostion);
        Debug.Log(transform.position);
    }
    //update card from Scriptable object.
    public void UpdateCard(Card data)
    {
        cardData = data;
        artwork.sprite = data.art;
        cardname.text = data.name;
        description.text = data.description;
        cost.text = data.energyCost.ToString();
    }
    //Movement
    public void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
        rb.velocity = Vector3.Normalize(destination - transform.position) * Vector3.Distance(destination, transform.position) / speed;
    }
    //Mouseover card. Should highlight potential effects. TODO: Zoom in card for better detail.
    public void OnPointerEnter(PointerEventData p)
    {
        Debug.Log("enter");
        HighlightRange();
        //zoom in
    }
    //Mouseexit card.  unhighlihgts potential efefcts and should zoom out, if zoomed in.
    public void OnPointerExit(PointerEventData p)
    {
        if (!p.dragging)
        {
            game.map.ClearHighlight();
        }
        //zoom out
    }
    //Starting to drag Card. Highlihghts targeted square in red for clarity.
    public void OnDrag(PointerEventData p)
    {
        destination = Input.mousePosition;
        game.inputControl.SetInput(InputController.InputMode.CardCast);

        Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseClick, out hit))
        {
            game.map.Target(hit.point, cardData.etype, cardData.effectRange, HighlightTiles.TileType.Target, null);
        }
    }
    //TODO should detect if card is playable and above a threshold (if card is returned to hand, should not be played)
    public void OnEndDrag(PointerEventData p)
    {
        game.map.ClearHighlight();
        game.map.ClearTarget();
        destination = defaultLocation;
        game.inputControl.SetInput(InputController.InputMode.None);
    }
    //helper function to highlight potential effects.
    public void HighlightRange()
    {
        game.HighlightTargets(cardData.targetRange, HighlightTiles.TileType.Attack, null);
        if (!cardData.targetsTypes.Contains(Card.TargetType.Self)){
            game.UnHiglightTarget(1);
        }
    }
}
