using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static General;

public class MainMenu : MonoBehaviour
{
    public Tilemap backgroundTilemap;
    public TileBase backgroundTile;
    public GameObject menuUI;

    public void StartCross()
    {
        TurnManager.crossPlayer = new HumanPlayer { playerFigure = figure.cross };
        TurnManager.circlePlayer = new AIPlayer { playerFigure = figure.circle };

        StartGame();
    }

    public void StartCircle()
    {
        TurnManager.crossPlayer = new AIPlayer { playerFigure = figure.cross };
        TurnManager.circlePlayer = new HumanPlayer { playerFigure = figure.circle };

        StartGame();
    }

    private void StartGame()
    {
        menuUI.SetActive(false);

        // Генерация поля.
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                backgroundTilemap.SetTile(tilePos, backgroundTile);

                tilePos += Vector3Int.right;
            }

            tilePos.x = 0;
            tilePos += Vector3Int.down;
        }

        TurnManager.NextTurn();
    }

}
