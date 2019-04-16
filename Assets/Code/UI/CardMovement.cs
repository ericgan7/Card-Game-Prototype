using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

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

    public void SetPosition(Vector3 newPostion)
    {
        defaultLocation = newPostion;
        tj.target = newPostion;
        tj.enabled = true;
        destination = newPostion;
    }

    public void UpdateCard(Card data)
    {
        cardData = data;
        artwork.sprite = data.art;
        cardname.text = data.name;
        description.text = data.description;
        cost.text = data.energyCost.ToString();
    }

    public void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * speed);
        rb.velocity = Vector3.Normalize(destination - transform.position) * Vector3.Distance(destination, transform.position) / speed;
    }

    public void OnPointerEnter(PointerEventData p)
    {
        Debug.Log("enter");
        HighlightRange();
        //zoom in
    }

    public void OnPointerExit(PointerEventData p)
    {
        if (!p.dragging)
        {
            game.map.ClearHighlight();
        }
        //zoom out
    }

    public void OnDrag(PointerEventData p)
    {
        destination = Input.mousePosition;
        game.inputControl.SetInput(InputController.InputMode.CardCast);

        Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(mouseClick, out hit))
        {
            game.map.Target(hit.point, cardData.etype, cardData.effectRange, HighlightTiles.TileType.Target);
        }
    }

    public void OnEndDrag(PointerEventData p)
    {
        game.map.ClearHighlight();
        game.map.ClearTarget();
        destination = defaultLocation;
    }

    public void HighlightRange()
    {
        game.HighlightTargets(cardData.targetRange, HighlightTiles.TileType.Attack);
        if (!cardData.targetsTypes.Contains(Card.TargetType.Self)){
            game.UnHiglightTarget(1);
        }
    }
}
