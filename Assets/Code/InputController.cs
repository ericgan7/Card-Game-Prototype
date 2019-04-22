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
        None, Movement, KeepCardSelect, Card
    }
    public InputMode mode;

    public bool disableInput;

    private void Start()
    {
        game = FindObjectOfType<GameController>();
        mode = InputMode.None;
        previousTile = new Vector3Int(-1, -1, -1);
        disableInput = false;
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
        if (disableInput)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseClick, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }
                switch (mode)
                {
                    case InputMode.None:
                        Character selected = game.map.GetCharacter(game.map.WorldToCellSpace(hit.point));
                        if(selected)
                        {
                            game.ui.SelectCharacter(selected);
                            if (selected == game.currentCharacter)
                            {
                                game.map.ClearHighlight();
                                game.ui.ActivateRadialMenu(game.currentCharacter.transform.position);
                                //update stat card
                            } else
                            {
                                game.map.Highlight(selected.transform.position, selected.GetSpeed(), HighlightTiles.TileType.Move, selected.stats.moveableTiles);
                                game.ui.DeactivateRadialMenu();
                            }
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
        }
        else if (Input.GetMouseButtonUp(1))
        {
            switch (mode)
            {
                case InputMode.KeepCardSelect:

                    break;
                default:
                    ResetInputState();
                    break;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            switch (mode)
            {
                case InputMode.KeepCardSelect:
                    game.hand.FinishCardToKeepSelection();
                    ResetInputState();
                    break;
                default:
                    break;
            }
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
        game.currentCharacter.ClearPath();
        game.map.ClearHighlight();
        game.map.ClearTarget();
    }


    //Handles movement selection for current Character. TODO: Add traveled path to target map and disallow backtracking?
    void CheckMovement(RaycastHit hit)
    {
        Vector3Int location = game.map.WorldToCellSpace(hit.point);
        //Activate movement after confirmation;
        if (previousTile.x >= 0 && previousTile == location)
        {
            mode = InputMode.None;
            game.currentCharacter.Move();
            //energy reduced by 1 in character script, check if energy is 0 and end turn if true
            if (!game.currentCharacter.HasEnergy())
            {
                List<Card> kept = new List<Card>();
                game.EndAllyTurn(kept); //keep nothing
            }
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
            previousTile = location;
            if (game.map.highlights.tilesFilled.ContainsKey(location))
            {
                if (game.currentCharacter.destinations.Count == 0)
                {
                    game.currentCharacter.AddPath(game.map.FindPath(game.map.WorldToCellSpace(game.currentCharacter.transform.position), location));
                }
                else
                {
                    game.currentCharacter.AddPath(game.map.FindPath(game.currentCharacter.destinations[0], location));
                }
            }
            game.map.ClearHighlight();
            game.map.Highlight(game.currentCharacter.destinations[0], game.currentCharacter.GetSpeed(), HighlightTiles.TileType.Move, game.currentCharacter.stats.moveableTiles);
        }
    }
}
