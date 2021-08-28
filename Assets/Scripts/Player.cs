using System.Collections;
using UnityEngine;
using static General;


public abstract class Player : MonoBehaviour
{
    [HideInInspector] public Figure playerFigure;
    [SerializeField] protected General general;
    [SerializeField] protected TurnManager turnManager;

    public abstract IEnumerator Act();

}
