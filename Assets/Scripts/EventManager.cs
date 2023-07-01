using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void Delegate_OnPing(string labelID);
    public static Delegate_OnPing OnPing;

    public static void Call_OnPing(string labelID)
    {
        OnPing?.Invoke(labelID);
    }
}
