﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static TurnManager;

public class General : MonoBehaviour
{
    public static General general;

    public Tilemap figureTileMap;
    public TileBase CircleTile, CrossTile;

    public const int fieldSize = 3;
    public const int winAmount = 3;

    // Набор всех направлений для проверки рядов фигур.
    public Vector3Int[] directions = {Vector3Int.up, Vector3Int.up + Vector3Int.right,
        Vector3Int.right, Vector3Int.right + Vector3Int.down,
        Vector3Int.down, Vector3Int.down + Vector3Int.left,
        Vector3Int.left, Vector3Int.left + Vector3Int.up };

    public enum figure { cross, circle, empty }

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

    public static figure CheckFigure(Vector3Int cellPosition)
    {
        TileBase choosenTile = general.figureTileMap.GetTile(cellPosition);
        figure figure;

        // Switch case тут не срабтает.
        if (choosenTile == general.CircleTile)
        {
            figure = figure.circle;
        }
        else if (choosenTile == general.CrossTile)
        {
            figure = figure.cross;
        }
        else
        {
            figure = figure.empty;
        }

        return figure;
    }


    public static void SetFigure(figure figure, Vector3Int cellPosition)
    {
        TileBase newTile = null;

        switch (figure)
        {
            case figure.cross:
                newTile = general.CrossTile;
                break;
            case figure.circle:
                newTile = general.CircleTile;
                break;
            case figure.empty:
                return;
        }

        general.figureTileMap.SetTile(cellPosition, newTile);
    }

    public static bool CheckWinCondition()
    {

        Vector3Int tilePos = new Vector3Int(0, 0, 0);

        for (int i = 0; i < fieldSize; i++)
        {
            for (int j = 0; j < fieldSize; j++)
            {
                figure figure = CheckFigure(tilePos);

                if (figure != figure.empty)
                {
                    foreach (Vector3Int direction in general.directions)
                    {
                        if (CountRow(tilePos, direction) == winAmount)
                        {
                            return true;
                        }
                    }
                }

                tilePos += Vector3Int.right;
            }

            tilePos.x = 0;
            tilePos += Vector3Int.down;
        }

        return false;
    }

    // Возвращает количество элементов в ряд.
    public static int CountRow(Vector3Int origin, Vector3Int direction)
    {
        int amount = 0;
        figure comparedFigure = CheckFigure(origin);
        figure newFigure;

        do
        {
            origin += direction;
            newFigure = CheckFigure(origin);
            if (newFigure == comparedFigure)
            {
                amount++;
            }
        } while (newFigure == comparedFigure);

        return amount;
    }
}