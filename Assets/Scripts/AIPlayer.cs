using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static General;


public class AIPlayer : Player
{

    public override IEnumerator Act()
    {
        Debug.Log("Ход компьютера");

        yield return new WaitForSeconds(1);
        Figure opponentFigure = (playerFigure == Figure.circle ? Figure.cross : Figure.circle);

        List<(int, int)> aviableTurns = new List<(int, int)>();
        List<(int, int)> criticalTurn = new List<(int, int)>();
        (int, int) choosenTurn;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                if (CheckFigure(tilePosition) == Figure.empty)
                {
                    if (SuggestTurn(playerFigure, (i, j)))
                    {
                        // Победный ход.
                        choosenTurn = (i, j);
                        goto setTile;
                    }
                    else if (SuggestTurn(opponentFigure, (i, j)))
                    {
                        // Ход предотвращающий победу соперника.
                        criticalTurn.Add((i, j));
                    }
                    else
                    {
                        // Случайный ход из доступных.
                        aviableTurns.Add((i, j));
                    }
                }
            }
        }

        if (criticalTurn.Count > 0)
        {
            int choosenIndex = Random.Range(0, criticalTurn.Count);
            choosenTurn = criticalTurn[choosenIndex];
        }
        else
        {
            int choosenIndex = Random.Range(0, aviableTurns.Count);
            choosenTurn = aviableTurns[choosenIndex];
        }

    setTile:

        general.SetFigure(playerFigure, choosenTurn);

        turnManager.MakeNextTurn();
    }

    private bool SuggestTurn(Figure figure, (int, int) tilePosition)
    {
        general.SetFigure(figure, tilePosition);
        bool answer = CheckWinCondition(out _);
        general.SetFigure(Figure.empty, tilePosition);

        return answer;
    }
}
