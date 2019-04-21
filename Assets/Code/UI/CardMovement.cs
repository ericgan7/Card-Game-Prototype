using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class used for card handling. Can be given a Card to set the art and and effect when played.
/// </summary>

public class CardMovement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public GameController game;
    public Card cardData;
    public Vector3 destination;
    Vector3 defaultLocation;
    public float speed;

    public Image artwork;
    public TextMeshProUGUI cardname;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;

    public float playableYPosition;
    public bool keepingMode;
    public bool isCardDrawn;

    public void Start()
    {
        destination = transform.localPosition;
        defaultLocation = transform.localPosition;
        UpdateCard(cardData);
        game = FindObjectOfType<GameController>();
        playableYPosition = 155f;
    }

    //Set position, used to draw from deck into hand.
    public void SetPosition(Vector3 newPostion)
    {
        defaultLocation = newPostion;
        destination = newPostion;
    }
    //update card from Scriptable object.
    public void UpdateCard(Card data)
    { 
        cardData = data;
        artwork.sprite = data.art;
        cardname.text = data.name;
        description.text = data.description;
        cost.text = data.energyCost.ToString();
        isCardDrawn = true;
    }
    //Movement
    public void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, destination, Time.deltaTime * speed);
    }
    //Mouseover card. Should highlight potential effects. TODO: Zoom in card for better detail.
    public void OnPointerEnter(PointerEventData p)
    {
        if (game.inputControl.mode != InputController.InputMode.KeepCardSelect)
        {
            HighlightRange();
        }
        //zoom in
    }

    public void OnPointerClick(PointerEventData p)
    {
        if (keepingMode)
        {
            game.hand.SelectCardToKeep(this);
        }
    }

    //Mouse exit card.  unhighlihgts potential efefcts and should zoom out, if zoomed in.
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
        game.ui.DeactivateRadialMenu();
        game.inputControl.SetInput(InputController.InputMode.Card);
        Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseClick, out hit))
        {
            game.map.Target(hit.point, cardData.etype, cardData.effectRange, HighlightTiles.TileType.Target, new List<Card.TargetType>(cardData.targetsTypes));
        }
    }
    //TODO should detect if card is playable and above a threshold (if card is returned to hand, should not be played)
    public void OnEndDrag(PointerEventData p)
    {
        bool successfulPlay = false;
        Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseClick, out hit))
        {
            successfulPlay = game.Cast(cardData);
            
        }
        if (successfulPlay)
        {
            //discard positioning TO DO:
            game.hand.DiscardCard();
        }
        else
        {
            destination = defaultLocation;
        }
        game.inputControl.SetInput(InputController.InputMode.None);
        game.map.ClearHighlight();
        game.map.ClearTarget();

    }
    //helper function to highlight target Tiles;
    public void HighlightRange()
    {
        game.HighlightTargets(cardData.targetRange, HighlightTiles.TileType.Attack, new List<Card.TargetType>(cardData.targetsTypes));
        if (!cardData.targetsTypes.Contains(Card.TargetType.Self)){
            game.UnHiglightTarget(1);
        }
    }
}
