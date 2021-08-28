using UnityEngine;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{
    public Tilemap figureTileMap, backgroundTilemap;
    public TileBase circleTile, crossTile, backgroundTile;

    public static int fieldSize = 3;
    public const int winAmount = 3;

    public enum Figure { cross, circle, empty }
    public static Figure[,] gameField;


    public static Figure CheckFigure(Vector3Int tilePosition)
    {
        return gameField[tilePosition.x, tilePosition.y];
    }

    public void SetFigure(Figure figure, Vector3Int tilePosition)
    {
        gameField[tilePosition.x, tilePosition.y] = figure;

        TileBase newTile = null;
        tilePosition.z = 0;

        switch (figure)
        {
            case Figure.cross:
                newTile = crossTile;
                break;
            case Figure.circle:
                newTile = circleTile;
                break;
            case Figure.empty:
                newTile = null;
                break;
        }

        figureTileMap.SetTile(tilePosition, newTile);
    }

    public void SetFigure(Figure figure, (int, int) tileIndex)
    {
        Vector3Int tilePosition = new Vector3Int(tileIndex.Item1, tileIndex.Item2, 0);
        gameField[tileIndex.Item1, tileIndex.Item2] = figure;

        TileBase newTile = null;

        switch (figure)
        {
            case Figure.cross:
                newTile = crossTile;
                break;
            case Figure.circle:
                newTile = circleTile;
                break;
            case Figure.empty:
                newTile = null;
                break;
        }

        figureTileMap.SetTile(tilePosition, newTile);
    }


    public static bool CheckWinCondition(out Figure winFigure)
    {
        // Ничья если ходов не осталось.
        bool emptyLeft = false;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                Figure figure = CheckFigure(tilePosition);

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
                            Vector3Int newTilePosition = new Vector3Int(i, newJPos, 0);
                            if (CheckFigure(newTilePosition) == figure)
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
                            Vector3Int newTilePosition = new Vector3Int(newIPos, j, 0);
                            if (CheckFigure(newTilePosition) == figure)
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
                            Vector3Int newTilePosition = new Vector3Int(newIPos, newJPos, 0);
                            if (CheckFigure(newTilePosition) == figure)
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
                            Vector3Int newTilePosition = new Vector3Int(newIPos, newJPos, 0);
                            if (CheckFigure(newTilePosition) == figure)
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
