using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static General;

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

    [Space]
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

        SetFieldSize();
        general.SetMode();
        general.generateField();

        turnManager.MakeNextTurn();
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

        general.deleteField();

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

        general.SetMode();
    }
    public void Toggle3DMode(Toggle toggle)
    {
        Mode2DToggle.isOn = !toggle.isOn;
        gameMode2D = !toggle.isOn;

        general.SetMode();
    }

}
