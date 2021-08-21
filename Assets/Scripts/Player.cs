using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;
using static General;


public abstract class Player
{
    public figure playerFigure;

    public abstract IEnumerator Act();

}
