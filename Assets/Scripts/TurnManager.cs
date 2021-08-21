using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
        
        switch (gameState)
        {
            case states.start:
                gameState = states.crossTurn;
                crossPlayer.StartCoroutine(crossPlayer.Act());
                break;

            case states.crossTurn:
                if (General.CheckWinCondition())
                {
                    gameState = states.finish;
                    Debug.Log("Победа крестика");
                    // Победа крестика.
                    break;
                }
                else
                {
                    gameState = states.circleTurn;
                    circlePlayer.StartCoroutine(circlePlayer.Act());
                    break;
                }


            case states.circleTurn:
                if (General.CheckWinCondition())
                {
                    gameState = states.finish;
                    Debug.Log("Победа нолика");
                    // Победа нолика.
                    break;
                }
                else
                {
                    gameState = states.crossTurn;
                    crossPlayer.StartCoroutine(crossPlayer.Act());
                    break;
                }


            default:
                break;
        }
    }
}
