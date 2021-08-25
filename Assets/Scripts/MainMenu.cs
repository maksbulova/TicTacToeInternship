using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static General;

public class MainMenu : MonoBehaviour
{

    public Player humanPlayer, aiPlayer;
    public Tilemap backgroundTilemap;
    public TileBase backgroundTile;
    public GameObject menuUI;

    public void StartCross()
    {
        TurnManager.crossPlayer = humanPlayer;
        humanPlayer.playerFigure = Figure.cross;

        TurnManager.circlePlayer = aiPlayer;
        aiPlayer.playerFigure = Figure.circle;

        StartGame();
    }

    public void StartCircle()
    {
        TurnManager.crossPlayer = aiPlayer;
        aiPlayer.playerFigure = Figure.cross;

        TurnManager.circlePlayer = humanPlayer;
        humanPlayer.playerFigure = Figure.circle;

        StartGame();
    }

    private void StartGame()
    {
        menuUI.SetActive(false);

        // Генерация поля.
        General.gameField = new Figure[fieldSize, fieldSize];
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                gameField[i, j] = Figure.empty;
                backgroundTilemap.SetTile(tilePos, backgroundTile);

                tilePos += Vector3Int.right;
            }

            tilePos.x = 0;
            tilePos += Vector3Int.up;
        }

        Debug.Log("Начало раунда");

        TurnManager.NextTurn();
    }

}
