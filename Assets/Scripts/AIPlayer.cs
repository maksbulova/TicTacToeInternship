using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static General;


public class AIPlayer : Player
{

    // Бот не просчитывает ходы наперед, лишь выбирает лучший на данном ходу.
    public override IEnumerator Act()
    {
        Debug.Log("Ход компьютера");

        yield return new WaitForSeconds(1);
        Figure opponentFigure = (playerFigure == Figure.circle ? Figure.cross : Figure.circle);

        List<(int, int)> aviableTurns = new List<(int, int)>();
        (int, int) choosenTile;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                if (CheckFigure((i, j)) == Figure.empty)
                {
                    if (SuggestTurn(playerFigure, (i, j)))
                    {
                        // Победный ход.
                        choosenTile = (i, j);
                        goto setTile;
                    }
                    else if (SuggestTurn(opponentFigure, (i, j)))
                    {
                        // Ход предотвращающий победу соперника.
                        choosenTile = (i, j);
                        goto setTile;
                    }
                    else
                    {
                        // Случайный ход из доступных.
                        aviableTurns.Add((i, j));
                    }
                }
            }
        }

        int choosenIndex = Random.Range(0, aviableTurns.Count);
        choosenTile = aviableTurns[choosenIndex];

    setTile:

        SetFigure(playerFigure, choosenTile);

        TurnManager.NextTurn();
    }

    private bool SuggestTurn(Figure figure, (int, int) tilePosition)
    {
        SetFigure(figure, tilePosition);
        bool answer = CheckWinCondition(out _);
        SetFigure(Figure.empty, tilePosition);

        return answer;
    }
}
