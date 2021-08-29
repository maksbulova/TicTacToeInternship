using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static General;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private Player player1, player2;
    [SerializeField] private GameObject menuScreen, resultScreen;
    [SerializeField] private Text resultText;
    [SerializeField] private Slider fieldSizeSlider;
    [SerializeField] private General general;
    [SerializeField] private TurnManager turnManager;

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

        GenerateField(fieldSize);

        turnManager.MakeNextTurn();
    }

    private void GenerateField(int fieldSize)
    {
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

        // TODO focus camera on field
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
        switch (fieldSizeSlider.value)
        {
            case 0:
                
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;

            default:
                break;
        }
    }
}
