using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static General;

public class TurnManager : MonoBehaviour
{
    public static Player crossPlayer, circlePlayer;

    public enum states
    {
        start, crossTurn, circleTurn, finish
    }

    public static states gameState;

    private void Start()
    {
        gameState = states.start;
    }

    public static void NextTurn()
    {

        if (General.CheckWinCondition(out Figure winnerFigure))
        {
            gameState = states.finish;

            Debug.Log($"Победа {winnerFigure}");
        }


        switch (gameState)
        {
            case states.start:
                gameState = states.crossTurn;
                crossPlayer.StartCoroutine(crossPlayer.Act());
                break;

            case states.crossTurn:
                gameState = states.circleTurn;
                circlePlayer.StartCoroutine(circlePlayer.Act());
                break;

            case states.circleTurn:
                gameState = states.crossTurn;
                crossPlayer.StartCoroutine(crossPlayer.Act());
                break;

            case states.finish:
                break;

            default:
                break;
        }
    }
}
