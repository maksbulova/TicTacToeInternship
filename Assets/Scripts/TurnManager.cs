using UnityEngine;
using static General;

public class TurnManager : MonoBehaviour
{
    public static Player crossPlayer, circlePlayer;
    [SerializeField] private MainMenu mainMenuManager;

    public enum States
    {
        start, crossTurn, circleTurn, finish
    }

    public static States gameState;

    public void MakeNextTurn()
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
                StartCoroutine(mainMenuManager.ShowGameOverScreen(winnerFigure));
                break;

            default:
                break;
        }
    }
}
