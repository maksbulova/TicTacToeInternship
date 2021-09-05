using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static General;

[ExecuteAlways]
public class MainMenu : MonoBehaviour
{
    [SerializeField] private Player player1, player2;

    [Space]
    [SerializeField] private GameObject menuScreen, resultScreen;
    [SerializeField] private Text resultText;

    [Space]
    [SerializeField] private Slider fieldSizeSlider;
    [SerializeField] private Text fieldSizeText;

    [SerializeField] private Toggle Mode2DToggle;
    [SerializeField] private Toggle Mode3DToggle;
    private bool gameMode2D = true;

    [Space]
    [SerializeField] private General general;
    [SerializeField] private TurnManager turnManager;
    [SerializeField] private Camera mainCamera;

    private const float resultScreenDuration = 2;

    private void Start()
    {
        menuScreen.SetActive(true);
        resultScreen.SetActive(false);
    }

    // Function called from user interface.
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

        GenerateField();

        turnManager.MakeNextTurn();
    }

    [ContextMenu("Generate Field")]
    private void GenerateField()
    {
        SetFieldSize();

        gameField = new Figure[fieldSize, fieldSize];
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                gameField[i, j] = Figure.empty;
                general.backgroundTilemap.SetTile(tilePos, general.backgroundTile);

                tilePos += Vector3Int.right;
            }

            tilePos.x = 0;
            tilePos += Vector3Int.up;
        }

        // Focus camera on field;
        float fieldCenter = ((float) fieldSize) / 2;
        const float cameraScaleCooficient = 0.66f;

        Vector3 cameraPosition = new Vector3(fieldCenter, fieldCenter, -10);
        mainCamera.transform.position = cameraPosition;

        mainCamera.orthographicSize =  fieldSize * cameraScaleCooficient;
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

        yield return new WaitForSeconds(resultScreenDuration);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                Vector3Int tilePosition = new Vector3Int(i, j, 0);
                general.figureTileMap.SetTile(tilePosition, null);
                general.backgroundTilemap.SetTile(tilePosition, null);
            }
        }

        resultScreen.SetActive(false);
        menuScreen.SetActive(true);
    }

    public void SetFieldSize()
    {
        int fieldSize;

        switch (fieldSizeSlider.value)
        {
            case 0:
                fieldSize = 3;
                break;
            case 1:
                fieldSize = 5;
                break;
            case 2:
                fieldSize = 9;
                break;
            case 3:
                fieldSize = 15;
                break;
            default:
                fieldSize = 3;
                break;
        }

        fieldSizeText.text = $"Field size:\n{fieldSize}x{fieldSize}";

        General.fieldSize = fieldSize;
    }

    public void Toggle2DMode(Toggle toggle)
    {
        Mode3DToggle.isOn = !toggle.isOn;
        gameMode2D = toggle.isOn;
    }
    public void Toggle3DMode(Toggle toggle)
    {
        Mode2DToggle.isOn = !toggle.isOn;
        gameMode2D = !toggle.isOn;
    }

}
