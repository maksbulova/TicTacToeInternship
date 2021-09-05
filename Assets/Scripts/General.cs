using UnityEngine;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{
    public Tilemap figureTileMap, backgroundTilemap;
    public TileBase circleTile, crossTile, backgroundTile;

    public GameObject circle3DModel, cross3DModel, background3DModel;

    public static int fieldSize = 3;

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
        Figure outputFigure = Figure.empty;

        bool CheckLine(Vector2Int lineOrigin, Vector2Int lineDirection)
        {
            Figure figure = CheckFigure(lineOrigin);

            if (figure != Figure.empty)
            {
                for (int offset = 1; offset < fieldSize; offset++)
                {
                    Vector2Int tilePosition = lineOrigin + lineDirection * offset;

                    Figure newFigure = CheckFigure(tilePosition);

                    if (newFigure == figure)
                    {
                        if (offset + 1 == fieldSize)
                        {
                            outputFigure = figure;
                            return true;
                        }

                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return false;

        }


        bool CheckAllLinesInColumn(Vector2Int columnDirection, Vector2Int lineDirection)
        {
            for (int offset = 0; offset < fieldSize; offset++)
            {
                Vector2Int checkedTile = columnDirection * offset;

                if (CheckLine(checkedTile, lineDirection))
                    return true;
            }

            return false;
        }

        bool CheckMatrix()
        {
            bool verticalCheck = CheckAllLinesInColumn(Vector2Int.up, Vector2Int.right);
            bool horizontalCheck = CheckAllLinesInColumn(Vector2Int.right, Vector2Int.up);
            bool diagonal1Check = CheckLine(Vector2Int.zero, Vector2Int.up + Vector2Int.right);
            bool diagonal2Check = CheckLine(Vector2Int.right * (fieldSize - 1), Vector2Int.up + Vector2Int.left);

            if (verticalCheck || horizontalCheck || diagonal1Check || diagonal2Check)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if (CheckMatrix())
        {
            winFigure = outputFigure;
            return true;
        }
        else
        {
            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    if (gameField[x, y] == Figure.empty)
                    {
                        winFigure = outputFigure;
                        return false;

                    }
                }
            }

            // Draw if no turns left.
            winFigure = outputFigure;
            return true;
        }
    }

}
