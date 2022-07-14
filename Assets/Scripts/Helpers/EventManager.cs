using System;
using UnityEngine;

public static class EventManager 
{
    public static event Action<Vector3> OnChangingPosOfPointer;
    public static void ChangingPosOfPointer(Vector3 posOfFinger) => OnChangingPosOfPointer?.Invoke(posOfFinger);
    public static event Action<Vector3> OnPointerUp;
    public static void PointerUp(Vector3 posOfFinger) => OnPointerUp?.Invoke(posOfFinger);

    public static event Action<Ball> OnBalFallenIntoHole;
    public static void BalFallenIntoHole(Ball fallenBall) => OnBalFallenIntoHole?.Invoke(fallenBall);
}
