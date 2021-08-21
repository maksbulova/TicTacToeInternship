using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static General;
using static TurnManager;

public class HumanPlayer : Player
{

    // Обработка действий игрока в его ход.
    public override IEnumerator Act()
    {
        Debug.Log("Ход игрока");

        states currentTurn = gameState;
        while (gameState == currentTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int cellPosition = general.figureTileMap.WorldToCell(pos);
                figure tileOnClick = CheckFigure(cellPosition);

                if (tileOnClick == figure.empty &&
                   (cellPosition.x >= 0 && cellPosition.x <= fieldSize) &&
                   (cellPosition.y <= 0 && cellPosition.y >= -fieldSize))
                {

                    SetFigure(playerFigure, cellPosition);
                    NextTurn();
                }
            }

            yield return null;
        }
    }
}
