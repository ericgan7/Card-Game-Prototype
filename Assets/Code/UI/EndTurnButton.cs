using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTurnButton : MonoBehaviour
{
	GameController game;

	private void Start(){
		game = FindObjectOfType<GameController>();
	}

    public void endTurn(){
        game.inputControl.EndTurn();
    }
 }
