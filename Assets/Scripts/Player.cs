using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TurnManager;
using static General;


public abstract class Player : MonoBehaviour
{
    [HideInInspector]
    public Figure playerFigure;

    public abstract IEnumerator Act();

}
