using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// InputController is used to detect player input and pass it onto the game controller.
/// </summary>

public class InputController : MonoBehaviour
{
    GameController game;

    public int xCameraMovementBuffer;
    public int yCameraMovementBuffer;
    public float cameraMovespeed;

    Vector3Int previousTile;

    public enum InputMode
    {
        None, Movement
    }
    InputMode mode;

    private void Start()
    {
        game = FindObjectOfType<GameController>();
        mode = InputMode.None;
        previousTile = new Vector3Int(-1, -1, -1);
    }

    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        //CameraMovement();
    }

    public void SetInput(InputMode m)
    {
        mode = m;
    }

    // Detetects player input.
        //currently only detects tile clicking movement. 
        //Card dragging is handled under CardMovement
    void PlayerInput()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseClick, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("ui");
                    return;
                }
                switch (mode)
                {
                    case InputMode.None:
                        Character selected = game.map.GetCharacter(game.map.WorldToCellSpace(hit.point));
                        if (selected == game.currentCharacter)
                        {
                            game.map.ClearHighlight();
                            game.ui.ActivateRadialMenu(Camera.main.WorldToScreenPoint(game.currentCharacter.transform.position));
                            //update stat card
                        }
                        else if (selected)
                        {
                            game.map.Highlight(selected.transform.position, selected.GetSpeed(), HighlightTiles.TileType.Move, new List<Card.TargetType> { Card.TargetType.Ground });
                            game.ui.DeactivateRadialMenu();
                        }
                        else
                        { 
                            game.ui.DeactivateRadialMenu();
                            ResetInputState();
                            //update stat card
                        }
                        break;
                    case InputMode.Movement:
                        CheckMovement(hit);
                        break;
                    default:
                        Debug.Log("Menu mode");
                        break;
                }
            }
            else
            {
                ResetInputState();
            }
        }
        else if (Input.GetMouseButtonUp(1))
        {
            ResetInputState();
        }
    }

    //Cmaera controllers. WASD to be addded for keyboard camera control.
    void CameraMovement()
    {
        Vector3 t = Camera.main.transform.position;
        if (Input.mousePosition.x > Screen.width - xCameraMovementBuffer)
        {
            t.x += cameraMovespeed;
            Camera.main.transform.position = t;
        }
        else if (Input.mousePosition.x < xCameraMovementBuffer)
        {
            t.x -= cameraMovespeed;
            Camera.main.transform.position = t;
        }
        if (Input.mousePosition.y > Screen.height - yCameraMovementBuffer)
        {
            t.y += cameraMovespeed;
            Camera.main.transform.position = t;
        }
        else if (Input.mousePosition.y < yCameraMovementBuffer)
        {
            t.y -= cameraMovespeed;
            Camera.main.transform.position = t;
        }
    }

    void ResetInputState()
    {
        Debug.Log("reset");
        game.ui.DeactivateRadialMenu();
        mode = InputMode.None;

        game.map.ClearHighlight();
        game.map.ClearTarget();
    }

    void CheckMovement(RaycastHit hit)
    {
        Vector3Int location = game.map.WorldToCellSpace(hit.point);
        //Activate movement after confirmation;
        if (previousTile.x >= 0 && previousTile == game.map.WorldToCellSpace(hit.point))
        {
            game.MoveCharacter(hit.point);
            mode = InputMode.None;
            game.map.ClearHighlight();
            previousTile = new Vector3Int(-1, -1, -1);
        }
        // Invalid MovementInput
        else if (location.x < 0 || !game.map.highlights.Contains(location))
        {
            ResetInputState();
        }
        //Record Valid Movement Input
        else
        {
            previousTile = game.map.WorldToCellSpace(hit.point);
        }
    }
}
