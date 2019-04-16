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
        None, Movement, CardCast
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
        CameraMovement();
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
        if (Input.GetMouseButtonDown(0))
        {
            Ray mouseClick = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseClick, out hit))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("ui");
                }
                switch (mode)
                {
                    case InputMode.None:
                        Debug.Log("click");
                        if (game.SelectLocation(hit.point))
                        {
                            mode = InputMode.Movement;
                        }
                        break;
                    case InputMode.Movement:
                        if (previousTile.x > 1 && previousTile == game.map.WorldToCellSpace(hit.point))
                        {
                            game.MoveCharacter(hit.point);
                            mode = InputMode.None;
                            game.map.ClearHighlight();
                            previousTile = new Vector3Int(-1, -1, -1);
                        }
                        else
                        {
                            previousTile = game.map.WorldToCellSpace(hit.point);
                        }
                        break;
                    default:
                        Debug.Log("Error");
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            mode = InputMode.None;
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
}
