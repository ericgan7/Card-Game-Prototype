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
    public float cameraScrollspeed;
    public float centerTime;
    public Vector2 xCameraPos;
    public Vector2 yCameraPos;
    public Vector3 offset;

    Vector3Int previousTile;
    float elapsed;
    bool cameraControls;
    Vector3 destination;
    Vector3 origin;

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
        elapsed = 0f;
        cameraControls = true;
    }

    void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        if (cameraControls)
        {
            CameraMovement();
        }
        else
        {
            if (elapsed < centerTime)
            {
                elapsed += Time.deltaTime;
                Camera.main.transform.localPosition = Vector3.Lerp(origin, destination, elapsed / centerTime);
            }
            else
            {
                cameraControls = true;
            }
        }
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
                                game.map.Highlight(selected.transform.position, Card.RangeType.Area, selected.GetSpeed(), HighlightTiles.TileType.Move, selected.stats.moveableTiles);
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
        Vector3 t = Camera.main.transform.localPosition;
        t.z = Mathf.Clamp(t.z + Input.mouseScrollDelta.y * cameraScrollspeed, -40f, -5f);
        if (Input.mousePosition.x > Screen.width - xCameraMovementBuffer || Input.GetKey(KeyCode.D))
        {
            t.x = Mathf.Clamp(t.x + cameraMovespeed, xCameraPos.x, xCameraPos.y);
        }
        else if (Input.mousePosition.x < xCameraMovementBuffer || Input.GetKey(KeyCode.A))
        {
            t.x = Mathf.Clamp(t.x - cameraMovespeed, xCameraPos.x, xCameraPos.y);
        }
        if (Input.mousePosition.y > Screen.height - yCameraMovementBuffer || Input.GetKey(KeyCode.W))
        {
            t.y = Mathf.Clamp(t.y + cameraMovespeed, yCameraPos.x, yCameraPos.y);
        }
        else if (Input.mousePosition.y < yCameraMovementBuffer || Input.GetKey(KeyCode.S))
        {
            t.y = Mathf.Clamp(t.y - cameraMovespeed, yCameraPos.x, yCameraPos.y);
        }
        Camera.main.transform.localPosition = t;
    }

    public void CenterCamera(Vector3 position)
    {
        elapsed = 0f;
        cameraControls = false;
        position += offset;
        destination = position;
        origin = Camera.main.transform.localPosition;
    }

    void ResetInputState()
    {
        Debug.Log("reset");
        game.ui.DeactivateRadialMenu();
        previousTile = new Vector3Int(-1, 0, 0);
        mode = InputMode.None;
        game.currentCharacter.ClearPath();
        game.map.ClearHighlight();
        game.map.ClearTarget();
    }


    //Handles movement selection for current Character. TODO: Add traveled path to target map and disallow backtracking?
    //TODO: Known bug with reseting previous location. Unkown how to fix yet.
    void CheckMovement(RaycastHit hit)
    {
        Vector3Int location = game.map.WorldToCellSpace(hit.point);
        //current character location is not a valid movement tile
        if (location == game.map.WorldToCellSpace(game.currentCharacter.transform.position))
        {
            return;
        }
        //Activate movement after confirmation;
        else if (previousTile.x >= 0 && previousTile == location)
        {
            mode = InputMode.None;
            game.currentCharacter.Move();
            game.map.ClearHighlight();
            game.map.ClearTarget();
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
            game.map.Highlight(game.currentCharacter.destinations[0], Card.RangeType.Area, game.currentCharacter.GetSpeed(), HighlightTiles.TileType.Move, game.currentCharacter.stats.moveableTiles);
            game.map.TargetPath(game.currentCharacter.destinations, HighlightTiles.TileType.Path);
        }
    }

    public void EndTurn()
    {
        if (game.currentCharacter.team == Card.TargetType.Ally)
        {
            game.UpdateTurn(new List<Card>());
            game.hand.DiscardHand();
        }
    }
}
