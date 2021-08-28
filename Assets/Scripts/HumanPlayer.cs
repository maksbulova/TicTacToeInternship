using System.Collections;
using UnityEngine;
using static General;

public class HumanPlayer : Player
{

    // Handling user input in his turn only.
    public override IEnumerator Act()
    {
        TurnManager.States currentTurn = TurnManager.gameState;
        while (TurnManager.gameState == currentTurn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPosition = general.figureTileMap.WorldToCell(pos);
                Vector2Int cellPosition = new Vector2Int(clickPosition.x, clickPosition.y);

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
