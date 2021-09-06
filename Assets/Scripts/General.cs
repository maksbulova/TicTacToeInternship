using UnityEngine;
using UnityEngine.Tilemaps;

public class General : MonoBehaviour
{
    public Tilemap figureTileMap, backgroundTilemap;
    public TileBase circleTile, crossTile, backgroundTile;

    public GameObject circle3DModel, cross3DModel, background3DModel;

    [SerializeField] private Camera mainCamera;

    public static int fieldSize = 3;
    public static bool gameMode2D = true;


    private const float fieldZPosition = 0;
    private const float figuresZPosition = -0.5f;

    public enum Figure { cross, circle, empty }
    public static Figure[,] gameField;


    public static Figure CheckFigure(Vector2Int tilePosition)
    {
        return gameField[tilePosition.x, tilePosition.y];
    }

    public delegate void SetFigure(Figure figure, Vector2Int tilePosition);
    public SetFigure setFigure;

    public void SetFigure2D(Figure figure, Vector2Int figurePosition)
    {
        gameField[figurePosition.x, figurePosition.y] = figure;

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

        Vector3Int tilePosition = new Vector3Int(figurePosition.x, figurePosition.y, 0);
        figureTileMap.SetTile(tilePosition, newTile);
    }

    public void SetFigure3D(Figure figure, Vector2Int figurePosition)
    {
        gameField[figurePosition.x, figurePosition.y] = figure;

        GameObject newObject = null;

        switch (figure)
        {
            case Figure.cross:
                newObject = cross3DModel;
                break;
            case Figure.circle:
                newObject = circle3DModel;
                break;
            case Figure.empty:
                newObject = null;
                break;
        }

        if (newObject != null)
        {
            Vector3 objectPostion = new Vector3(figurePosition.x, figurePosition.y, figuresZPosition);
            Quaternion objectRotation = Quaternion.Euler(-90, 0, 0);
            GameObject newFigure = Instantiate(newObject, objectPostion, objectRotation);
            newFigure.tag = "Figure";
        }
        else
        {

            Vector3 rayOrigin = new Vector3(figurePosition.x, figurePosition.y, fieldZPosition + Vector3.forward.z);
            Ray ray = new Ray(rayOrigin, Vector3.back);
            // Debug.DrawRay(rayOrigin, Vector3.back, Color.red, 5f);

            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                GameObject gameObject = hit.collider.gameObject;
                if (gameObject.CompareTag("Figure"))
                {
                    Destroy(gameObject);
                }
            }

        }
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

    public delegate void GenerateField();
    public GenerateField generateField;

    public void GenerateField2D()
    {
        gameField = new Figure[fieldSize, fieldSize];
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

        FocusCamera();
    }

    public void GenerateField3D()
    {
        gameField = new Figure[fieldSize, fieldSize];
        Vector3 objectPosition = new Vector3(0, 0, fieldZPosition);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                gameField[i, j] = Figure.empty;

                Quaternion objectRotation = Quaternion.Euler(-90, 0, 0);
                Instantiate(background3DModel, objectPosition, objectRotation);

                objectPosition += Vector3.right;
            }

            objectPosition.x = 0;
            objectPosition += Vector3Int.up;
        }

        FocusCamera();
    }

    private void FocusCamera()
    {
        float fieldCenter = ((float)fieldSize) / 2;
        const float cameraScaleCooficient = 0.66f;

        Vector3 cameraPosition = new Vector3(fieldCenter, fieldCenter, -10);
        mainCamera.transform.position = cameraPosition;

        mainCamera.orthographicSize = fieldSize * cameraScaleCooficient;

    }

    public delegate void DeleteField();
    public DeleteField deleteField;

    public void DeleteField2D()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                figureTileMap.SetTile(tilePosition, null);
                backgroundTilemap.SetTile(tilePosition, null);
            }
        }
    }

    public void DeleteField3D()
    {
        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3 rayOrigin = new Vector3(i, j, fieldZPosition + Vector3.forward.z);
                Ray ray = new Ray(rayOrigin, Vector3.back);
                // Debug.DrawRay(rayOrigin, Vector3.back, Color.red, 5f);

                RaycastHit[] hits = Physics.RaycastAll(ray);

                foreach (RaycastHit hit in hits)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }


    public void SetMode()
    {
        if (gameMode2D)
        {
            setFigure = SetFigure2D;
            generateField = GenerateField2D;
            deleteField = DeleteField2D;
        }
        else
        {
            setFigure = SetFigure3D;
            generateField = GenerateField3D;
            deleteField = DeleteField3D;
        }
    }
}
