using UnityEngine;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{
    public Tilemap figureTileMap, backgroundTilemap;
    public TileBase circleTile, crossTile, backgroundTile;

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

        bool CheckLine(Vector2Int lineOrigin, bool diagonal = false)
        {
            Figure figure = CheckFigure(lineOrigin);

            if (figure != Figure.empty)
            {
                for (int xPos = 1; xPos < fieldSize; xPos++)
                {
                    int yPos = diagonal ? xPos : lineOrigin.y;
                    Vector2Int newTilePosition = new Vector2Int(xPos, yPos);

                    Figure newFigure = CheckFigure(newTilePosition);

                    if (newFigure == figure)
                    {
                        if (xPos + 1 == fieldSize)
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

            outputFigure = Figure.empty;
            return false;

        }


        bool CheckAllLines()
        {
            for (int yPos = 0; yPos < fieldSize; yPos++)
            {
                bool lineWinCondition;
                Vector2Int checkedTile = new Vector2Int(0, yPos);

                lineWinCondition = CheckLine(checkedTile);

                if (yPos == 0 && !lineWinCondition)
                {
                    lineWinCondition = CheckLine(checkedTile, diagonal: true);
                }

                if (lineWinCondition)
                {
                    return true;
                }
            }

            return false;
        }

        void TransposeField()
        {
            Figure[,] tempField = gameField;

            for (int x = 0; x < fieldSize; x++)
            {
                for (int y = 0; y < fieldSize; y++)
                {
                    gameField[x, y] = tempField[y, x];
                }
            }
        }

        if (CheckAllLines())
        {
            winFigure = outputFigure;
            return true;
        }
        else
        {
            TransposeField();

            if (CheckAllLines())
            {
                winFigure = outputFigure;
                return true;
            }
            TransposeField();
        }

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
