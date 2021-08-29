using UnityEngine;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{
    public Tilemap figureTileMap, backgroundTilemap;
    public TileBase circleTile, crossTile, backgroundTile;

    public static int fieldSize = 3;
    private static int winAmount;

    public enum Figure { cross, circle, empty }
    public static Figure[,] gameField;


    public static Figure CheckFigure(Vector2Int tilePosition)
    {
        return gameField[tilePosition.x, tilePosition.y];
    }

    public void SetFigure(Figure figure, Vector2Int tilePosition)
    {
        gameField[tilePosition.x, tilePosition.y] = figure;

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

        Vector3Int tilePositionV3 = new Vector3Int(tilePosition.x, tilePosition.y, 0);
        figureTileMap.SetTile(tilePositionV3, newTile);
    }


    public static bool CheckWinCondition(out Figure winFigure)
    {
        winAmount = fieldSize;

        // Draw if no turns left.
        bool emptyLeft = false;

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector2Int tilePosition = new Vector2Int(i, j);
                Figure figure = CheckFigure(tilePosition);

                if (figure == Figure.empty)
                {
                    emptyLeft = true;
                }
                else
                {
                    // Check horizontal line.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newJPos = j + k;

                        if (newJPos >= 0 && newJPos < fieldSize)
                        {
                            Vector2Int newTilePosition = new Vector2Int(i, newJPos);
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

                    // Check vertical line.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newIPos = i + k;

                        if (newIPos >= 0 && newIPos < fieldSize)
                        {
                            Vector2Int newTilePosition = new Vector2Int(newIPos, j);
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

                    // Check bottom left-top right diagonal line.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newIPos = i + k;
                        int newJPos = j + k;

                        if (newIPos >= 0 && newIPos < fieldSize &&
                            newJPos >= 0 && newJPos < fieldSize)
                        {
                            Vector2Int newTilePosition = new Vector2Int(newIPos, newJPos);
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

                    // Check bottom right-top left diagonal line.
                    for (int k = 1; k < winAmount; k++)
                    {
                        int newIPos = i + k;
                        int newJPos = j - k;

                        if (newIPos >= 0 && newIPos < fieldSize &&
                            newJPos >= 0 && newJPos < fieldSize)
                        {
                            Vector2Int newTilePosition = new Vector2Int(newIPos, newJPos);
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
            // Draw.
            return true;
        }

    }

}
