using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static General;

public class TurnManager : MonoBehaviour
{
    public static Player crossPlayer, circlePlayer;

    public enum States
    {
        start, crossTurn, circleTurn, finish
    }

    public static States gameState;

    private void Start()
    {
        gameState = States.start;
    }

    public static void NextTurn()
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
                break;

            default:
                break;
        }
    }
}
