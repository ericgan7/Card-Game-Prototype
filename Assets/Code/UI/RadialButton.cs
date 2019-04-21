﻿using System.Collections;
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
                menu.game.map.Highlight(target.transform.position, target.GetSpeed(), HighlightTiles.TileType.Move, target.stats.moveableTiles);
                break;
            case "EndTurn":
                menu.game.inputControl.SetInput(InputController.InputMode.None);
                menu.game.UpdateTurn();
                break;
            default:
                Debug.Log("ERROR - unknown radial button selected");
                break;
        }
        menu.game.ui.DeactivateRadialMenu();
    }
}
