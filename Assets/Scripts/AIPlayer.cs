using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static General;


public class AIPlayer : Player
{

    // Бот не просчитывает ходы наперед, лишь выбирает лучший на данном ходу.
    public override IEnumerator Act()
    {
        Debug.Log("Ход компьютера");

        yield return new WaitForSeconds(1);

        // int[,] tilesRating = new int[fieldSize, fieldSize];
        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        // Перечень самых выгодных ходов.
        List<Vector3Int> maxRatedTiles = new List<Vector3Int>();
        int maxRating = 0;


        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                int rating = 0;
                if (CheckFigure(tilePos) != figure.empty)
                {
                    // tilesRating[i, j] = 0;
                }
                else
                {
                    
                    foreach (Vector3Int direction in general.directions)
                    {
                        rating += CountRow(tilePos, direction)^2;
                    }
                    // tilesRating[i, j] = rating;

                    if (rating > maxRating)
                    {
                        maxRating = rating;
                        maxRatedTiles.Clear();
                        maxRatedTiles.Add(new Vector3Int(i, j, 0));
                    }
                    else if (rating == maxRating)
                    {
                        maxRatedTiles.Add(new Vector3Int(i, j, 0));
                    }

                }

                tilePos += Vector3Int.right;
            }

            tilePos.x = 0;
            tilePos += Vector3Int.down;
        }

        int choosenIndex = Random.Range(0, maxRatedTiles.Count);
        Vector3Int choosenTile = maxRatedTiles[choosenIndex];

        SetFigure(playerFigure, choosenTile);

        TurnManager.NextTurn();
    }
}
