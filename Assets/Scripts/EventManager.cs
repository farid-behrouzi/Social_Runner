using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void Delegate_General();
    public delegate void Delegate_OnPing(string labelID);
    public delegate void Delegate_OnTakeSnapshot(bool isPlayer);
    public delegate void Delegate_OnHit(/*TokenData _token*/);
    public static Delegate_OnPing OnPing;
    public static Delegate_OnTakeSnapshot OnTakeSnapshot;
    public static Delegate_OnHit OnHit;

    public static void Call_OnPing(string labelID)
    {
        OnPing?.Invoke(labelID);
    }

    public static void Call_OnTakeSnapshot(bool isPlayer)
    {
        OnTakeSnapshot?.Invoke(isPlayer);
    }

    public static void Call_OnHit()
    {
        OnHit?.Invoke();
    }
}
