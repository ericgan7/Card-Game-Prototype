using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RadialButton: MonoBehaviour, IPointerClickHandler
{
    public string actionName;
    public Image icon;
    public Image frame;
    public RadialMenu menu;

    public void OnPointerClick(PointerEventData p)
    {
        switch (actionName)
        {
            case "Move":
                menu.game.inputControl.SetInput(InputController.InputMode.Movement);
                Character target = menu.game.currentCharacter;
                menu.game.map.Highlight(target.transform.position, target.GetSpeed(), HighlightTiles.TileType.Move, new List<Card.TargetType> {Card.TargetType.Ground });
                break;
            case "Rest":
                menu.game.inputControl.SetInput(InputController.InputMode.None);
                //implementing rest;
                break;
            default:
                Debug.Log("ERROR - unknown radial button selected");
                break;
        }
        menu.game.ui.DeactivateRadialMenu();
    }
}
