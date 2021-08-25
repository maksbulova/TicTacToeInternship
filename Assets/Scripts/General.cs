using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{
    public static General general;

    public Tilemap figureTileMap;
    public TileBase circleTile, crossTile;

    public const int fieldSize = 3;
    public const int winAmount = 3;

    public enum Figure { cross, circle, empty }
    public static Figure[,] gameField;

    private void Awake()
    {
        // Я помню с первой лекции что синглтон лучше не использовать,
        // но удобной альтернативы перетаскивать в инспектор поля и
        // обращаться к ним из любого скрипта я не нашел.
        if (general != null)
        {
            Destroy(general);
        }
        else
        {
            general = this;
        }
    }

    private static (int, int) GetTileIndex(Vector3Int tilePosition)
    {
        return (tilePosition.x, tilePosition.y);
    }

    public static Figure CheckFigure(Vector3Int tilePosition)
    {
        (int, int) tileIndex = GetTileIndex(tilePosition);
        return CheckFigure(tileIndex);
    }

    public static Figure CheckFigure((int, int) tileIndex)
    {
        return gameField[tileIndex.Item1, tileIndex.Item2];
    }

    public static void SetFigure(Figure figure, Vector3Int tilePosition)
    {
        (int, int) tileIndex = GetTileIndex(tilePosition);

        gameField[tileIndex.Item1, tileIndex.Item2] = figure;

        TileBase newTile = null;
        tilePosition.z = 0;

        switch (figure)
        {
            case Figure.cross:
                newTile = general.crossTile;
                break;
            case Figure.circle:
                newTile = general.circleTile;
                break;
            case Figure.empty:
                newTile = null;
                break;
        }

        general.figureTileMap.SetTile(tilePosition, newTile);
    }

    public static void SetFigure(Figure figure, (int, int) tileIndex)
    {
        Vector3Int tilePosition = new Vector3Int(tileIndex.Item1, tileIndex.Item2, 0);
        gameField[tileIndex.Item1, tileIndex.Item2] = figure;

        TileBase newTile = null;

        switch (figure)
        {
            case Figure.cross:
                newTile = general.crossTile;
                break;
            case Figure.circle:
                newTile = general.circleTile;
                break;
            case Figure.empty:
                newTile = null;
                break;
        }

        general.figureTileMap.SetTile(tilePosition, newTile);
    }


    public static bool CheckWinCondition(out Figure winFigure)
    {
        // Ничья если ходов не осталось.
        bool emptyLeft = false;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Figure figure = CheckFigure((i, j));

                if (figure == Figure.empty)
                {
                    emptyLeft = true;
                }
                else
                {
                    // Проверка горизонтальной линии.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newJPos = j + k;

                        if (newJPos >= 0 && newJPos < fieldSize)
                        {
                            if (CheckFigure((i, newJPos)) == figure)
                            {
                                if (k + 1 == winAmount)
                                {
                                    winFigure = figure;
                                    return true;
                                }

                                continue;
                            }
                        }

                        break;
                    }

                    // Проверка вертикаьной линии.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newIPos = i + k;

                        if (newIPos >= 0 && newIPos < fieldSize)
                        {
                            if (CheckFigure((newIPos, j)) == figure)
                            {
                                if (k + 1 == winAmount)
                                {
                                    winFigure = figure;
                                    return true;
                                }

                                continue;
                            }
                        }

                        break;
                    }

                    // Проверка диагональной линии слева на право.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newIPos = i + k;
                        int newJPos = j + k;

                        if (newIPos >= 0 && newIPos < fieldSize &&
                            newJPos >= 0 && newJPos < fieldSize)
                        {
                            if (CheckFigure((newIPos, newJPos)) == figure)
                            {
                                if (k + 1 == winAmount)
                                {
                                    winFigure = figure;
                                    return true;
                                }

                                continue;
                            }
                        }

                        break;
                    }

                    // Проверка диагональной линии справа на лево.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newIPos = i + k;
                        int newJPos = j - k;

                        if (newIPos >= 0 && newIPos < fieldSize &&
                            newJPos >= 0 && newJPos < fieldSize)
                        {
                            if (CheckFigure((newIPos, newJPos)) == figure)
                            {
                                if (k + 1 == winAmount)
                                {
                                    winFigure = figure;
                                    return true;
                                }

                                continue;
                            }
                        }

                        break;
                    }

                }
            }
        }

        winFigure = Figure.empty;

        if (emptyLeft)
        {
            return false;
        }
        else
        {
            // Ничья.
            return true;
        }

    }

}
