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
        if (menu.game.currentCharacter.GetEnergy()[0] <= 0)
        {
            menu.game.ui.DeactivateRadialMenu();
            return;
        }
        else
        {
            switch (actionName)
            {
                case "Move":

                    menu.game.inputControl.SetInput(InputController.InputMode.Movement);
                    Character target = menu.game.currentCharacter;
                    menu.game.map.Highlight(target.transform.position, Card.RangeType.Area, target.GetSpeed(), HighlightTiles.TileType.Move, target.stats.moveableTiles);
                    break;
                case "EndTurn":

                    menu.game.hand.StartSelectionToKeep();
                    menu.game.inputControl.SetInput(InputController.InputMode.KeepCardSelect);

                    break;
                default:
                    Debug.Log("ERROR - unknown radial button selected");
                    break;
            }
        }
        menu.game.ui.DeactivateRadialMenu();
    }
}
