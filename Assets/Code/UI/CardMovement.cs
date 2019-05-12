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
    Vector3 targetScale;
    Vector3 defaultLocation;
    Vector3 discardLocation;
    Vector2 cameraSize;
    public float speed;

    public Image artwork;
    public TextMeshProUGUI cardname;
    public TextMeshProUGUI description;
    public TextMeshProUGUI cost;

    public bool keepingMode;
    public bool isCardDrawn;

    RectTransform hand;
    public GameObject play;
    Vector3 playPos;

    public int lineSmoothness;
    public float lineCurve;
    LineRenderer line;
    public float arrowsHeadSize;

    int siblingIndex;
    public void Start()
    {
        destination = transform.localPosition;
        defaultLocation = transform.localPosition;
        discardLocation = transform.localPosition;
        targetScale = Vector3.one;
        isCardDrawn = false;
        game = FindObjectOfType<GameController>();
        cameraSize = new Vector2(Camera.main.scaledPixelWidth / 2, 0);
        hand = transform.parent.gameObject.GetComponent<RectTransform>();
        playPos = play.transform.localPosition;
        line = GetComponent<LineRenderer>();
        line.sortingLayerName = "Foreground";
        line.sortingOrder = 5;
    }

    //Set position, used to draw from deck into hand.
    public void SetPosition(Vector3 newPostion)
    {
        defaultLocation = newPostion;
        destination = newPostion;
    }
    //update card from Scriptable object.
    public void UpdateCard(Card data, Character origin, int index)
    { 
        cardData = data;
        artwork.sprite = data.art;
        cardname.text = data.name;
        description.text = data.GetDescription(origin);
        cost.text = data.energyCost.ToString();
        isCardDrawn = true;
        targetScale = Vector3.one;
        siblingIndex = index;
    }
    //Movement
    public void FixedUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, destination, Time.deltaTime * speed);
        transform.localScale = Vector3.Slerp(transform.localScale, targetScale, Time.deltaTime * speed);
    }
    //Mouseover card. Should highlight potential effects. TODO: Zoom in card for better detail.
    public void OnPointerEnter(PointerEventData p)
    {
        if (!isCardDrawn)
        {
            return;
        }
        if (game.inputControl.mode != InputController.InputMode.KeepCardSelect)
        {
            HighlightRange();
            targetScale = new Vector3(2.0f, 2.0f, 2.0f);
            destination.y += 100f;
            transform.SetAsLastSibling();
        }

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
        targetScale = new Vector3(1.0f, 1.0f, 1.0f);
        destination = defaultLocation;
        game.inputControl.SetInput(InputController.InputMode.None);
        transform.SetSiblingIndex(siblingIndex);
    }
    //Starting to drag Card. Highlihghts targeted square in red for clarity.
    //TO DO:POLISH CARD DRAGGING
    public void OnDrag(PointerEventData p)
    {
        if (!isCardDrawn)
        {
            return;
        }
        //if untargetable, will always follow the mouse, since it does not need to be targeted on a specific target.
        Vector3 mousepos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Vector3.Distance(Camera.main.transform.position, transform.position)));
        destination = transform.parent.InverseTransformPoint(mousepos);
        game.ui.DeactivateRadialMenu();
        game.inputControl.SetInput(InputController.InputMode.Card);
        //if targetables
        if (cardData.etype == Card.EffectType.Targetable)
        {
            if (destination.y > hand.sizeDelta.y)
            {
                destination = playPos;
                Vector3 middle = transform.position;
                middle.y += lineCurve;
                line.positionCount = lineSmoothness;
                Vector3[] positions = game.ui.curve.QuadraticCurve(transform.position, middle, mousepos, lineSmoothness);
                line.SetPositions(positions);
                line.widthCurve = new AnimationCurve(
                    new Keyframe(0, 0.5f),
                    new Keyframe(0.99f - arrowsHeadSize, 0.5f),
                    new Keyframe(1f - arrowsHeadSize, 1f),
                    new Keyframe(1, 0f)
                );
            }
            else
            {
                line.positionCount = 0;
            }
         
            Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseClick, out hit))
            {
                game.map.Target(hit.point, cardData.rtype, cardData.effectRange, HighlightTiles.TileType.Target, new List<Card.TargetType>(cardData.targetsTypes));
                game.ui.displayText.DisplayTargets(cardData, game.currentCharacter);
            }
        }
        else
        {
            game.map.Target(game.currentCharacter.GetPosition(), cardData.rtype, cardData.effectRange, HighlightTiles.TileType.Target, new List<Card.TargetType>(cardData.targetsTypes));
            game.ui.displayText.DisplayTargets(cardData, game.currentCharacter);
        }
    }
    //TODO should detect if card is playable and above a threshold (if card is returned to hand, should not be played)
    public void OnEndDrag(PointerEventData p)
    {
        bool successfulPlay = false;
        if (cardData.etype == Card.EffectType.Nontargetable && destination.y > 200)
        {
            successfulPlay = game.Cast(cardData);
        }
        else
        {
            Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseClick, out hit))
            {
                successfulPlay = game.Cast(cardData);
            }
        }
        if (successfulPlay)
        {
            //discard positioning TO DO:
            GetComponent<Animator>().enabled = true;
            GetComponent<Animator>().Play("CardAnimation");
            CardEffect.instance.openUp();
        }
        else
        {
            destination = defaultLocation;
        }
        game.inputControl.SetInput(InputController.InputMode.None);
        game.map.ClearHighlight();
        game.map.ClearTarget();
        game.ui.displayText.ResetTargets();
        line.positionCount = 0;
    }
    //helper function to highlight target Tiles;
    public void HighlightRange()
    {
        game.HighlightTargets(cardData.rtype, cardData.targetRange, HighlightTiles.TileType.Attack, new List<Card.TargetType>(cardData.targetsTypes));
        if (!cardData.targetsTypes.Contains(Card.TargetType.Self)){
            game.UnHiglightTarget(1);
        }
    }
    //Only call at the end of card playing animation.
    public void Reset(int i = 0)
    {
        destination = discardLocation;
        GetComponent<Animator>().enabled = false;
        isCardDrawn = false;
        keepingMode = false;
    }

    public void Play()
    {
        game.PlayAction();
    }
}
