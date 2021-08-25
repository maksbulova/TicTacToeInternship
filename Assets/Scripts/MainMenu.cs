using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using static General;

public class MainMenu : MonoBehaviour
{

    public Player player1, player2;
    public Tilemap backgroundTilemap;
    public TileBase backgroundTile;
    public GameObject menuScreen, resultScreen;
    public Text resultText;

    private void Start()
    {
        menuScreen.SetActive(true);
        resultScreen.SetActive(false);
    }

    // Функция вызывается через интерфейс выбора фигуры.
    public void StartCross()
    {
        TurnManager.crossPlayer = player1;
        player1.playerFigure = Figure.cross;

        TurnManager.circlePlayer = player2;
        player2.playerFigure = Figure.circle;

        StartGame();
    }

    public void StartCircle()
    {
        TurnManager.crossPlayer = player2;
        player2.playerFigure = Figure.cross;

        TurnManager.circlePlayer = player1;
        player1.playerFigure = Figure.circle;

        StartGame();
    }

    private void StartGame()
    {
        TurnManager.gameState = TurnManager.States.start;

        menuScreen.SetActive(false);

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
        TurnManager.MakeNextTurn();
    }

    public IEnumerator ShowGameOverScreen(Figure winnerFigure)
    {
        resultScreen.SetActive(true);

        string text = "";
        switch (winnerFigure)
        {
            case Figure.cross:
                text = "Cross win!";
                break;
            case Figure.circle:
                text = "Circle win!";
                break;
            case Figure.empty:
                text = "Draw!";
                break;
            default:
                break;
        }

        resultText.text = text;

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                general.figureTileMap.SetTile(tilePosition, null);

                yield return new WaitForSeconds(0.2f);
            }
        }

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                backgroundTilemap.SetTile(tilePosition, null);

                yield return new WaitForSeconds(0.2f);
            }
        }

        yield return new WaitForSeconds(1f);

        resultScreen.SetActive(false);
        menuScreen.SetActive(true);
        
    }
}
