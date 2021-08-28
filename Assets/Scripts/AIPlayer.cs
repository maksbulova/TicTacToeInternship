using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static General;


public class AIPlayer : Player
{

    public override IEnumerator Act()
    {
        yield return new WaitForSeconds(1);
        Figure opponentFigure = (playerFigure == Figure.circle ? Figure.cross : Figure.circle);

        List<Vector2Int> avaibleTurns = new List<Vector2Int>();
        List<Vector2Int> criticalTurn = new List<Vector2Int>();
        Vector2Int choosenTurn;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector2Int tilePosition = new Vector2Int(i, j);
                if (CheckFigure(tilePosition) == Figure.empty)
                {
                    if (SuggestTurn(playerFigure, new Vector2Int(i, j)))
                    {
                        // Win turn.
                        choosenTurn = new Vector2Int(i, j);
                        goto setTile;
                    }
                    else if (SuggestTurn(opponentFigure, new Vector2Int(i, j)))
                    {
                        // Prevent opponents win.
                        criticalTurn.Add(new Vector2Int(i, j));
                    }
                    else
                    {
                        // Random avaible turn.
                        avaibleTurns.Add(new Vector2Int(i, j));
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
            int choosenIndex = Random.Range(0, avaibleTurns.Count);
            choosenTurn = avaibleTurns[choosenIndex];
        }

    setTile:

        general.SetFigure(playerFigure, choosenTurn);

        turnManager.MakeNextTurn();
    }

    private bool SuggestTurn(Figure figure, Vector2Int tilePosition)
    {
        general.SetFigure(figure, tilePosition);
        bool answer = CheckWinCondition(out _);
        general.SetFigure(Figure.empty, tilePosition);

        return answer;
    }
}
