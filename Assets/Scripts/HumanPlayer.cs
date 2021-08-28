using System.Collections;
using UnityEngine;
using static General;

public class HumanPlayer : Player
{

    // Обработка действий игрока в его ход.
    public override IEnumerator Act()
    {
        Debug.Log("Ход игрока");

        TurnManager.States currentTurn = TurnManager.gameState;
        while (TurnManager.gameState == currentTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = general.figureTileMap.WorldToCell(pos);
                cellPosition.z = 0;
                Figure tileOnClick = CheckFigure(cellPosition);

                if (tileOnClick == Figure.empty &&
                   (cellPosition.x >= 0 && cellPosition.x <= fieldSize) &&
                   (cellPosition.y >= 0 && cellPosition.y <= fieldSize))
                {

                    general.SetFigure(playerFigure, cellPosition);
                    turnManager.MakeNextTurn();
                }
            }

            yield return null;
        }
    }
}
