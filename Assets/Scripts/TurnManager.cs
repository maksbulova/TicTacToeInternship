using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static General;

public class TurnManager
{
    public static Player crossPlayer, circlePlayer;
    

    public enum States
    {
        start, crossTurn, circleTurn, finish
    }

    public static States gameState;


    public static void MakeNextTurn()
    {

        if (General.CheckWinCondition(out Figure winnerFigure))
        {
            gameState = States.finish;
            Debug.Log($"Победа {winnerFigure}");
        }


        switch (gameState)
        {
            case States.start:
                gameState = States.crossTurn;
                crossPlayer.StartCoroutine(crossPlayer.Act());
                break;

            case States.crossTurn:
                gameState = States.circleTurn;
                circlePlayer.StartCoroutine(circlePlayer.Act());
                break;

            case States.circleTurn:
                gameState = States.crossTurn;
                crossPlayer.StartCoroutine(crossPlayer.Act());
                break;

            case States.finish:
                General.general.mainMenu.StartCoroutine(General.general.mainMenu.ShowGameOverScreen(winnerFigure));
                break;

            default:
                break;
        }
    }
}
