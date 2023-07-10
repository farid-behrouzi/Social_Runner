using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    public delegate void Delegate_General();
    public delegate void Delegate_OnPing(string labelID);
    public delegate void Delegate_OnTakeSnapshot(bool isPlayer);
    public delegate void Delegate_OnHit(Token token);
    public delegate void Delegate_OnTrendChange(bool state);
    public delegate void Delegate_OnLevelUp(int level, int points);
    public delegate void Delegate_OnScore(int score);
    public static Delegate_OnPing OnPing;
    public static Delegate_OnTakeSnapshot OnTakeSnapshot;
    public static Delegate_OnHit OnHit;
    public static Delegate_OnTrendChange OnTrendChange;
    public static Delegate_OnLevelUp OnLevelUp;
    public static Delegate_General OnEnd;
    public static Delegate_General OnStart;
    public static Delegate_General OnStopWheel;
    public static Delegate_General OnSlowWheel;
    public static Delegate_OnScore OnScore;

    public static void Call_OnPing(string labelID)
    {
        OnPing?.Invoke(labelID);
    }

    public static void Call_OnTakeSnapshot(bool isPlayer)
    {
        OnTakeSnapshot?.Invoke(isPlayer);
    }

    public static void Call_OnHit(Token token)
    {
        OnHit?.Invoke(token);
    }

    public static void Call_OnTrendChange(bool state)
    {
        OnTrendChange?.Invoke(state);
    }

    public static void Call_OnLevelUp(int level, int points)
    {
        OnLevelUp?.Invoke(level, points);
    }

    public static void Call_End()
    {
        OnEnd?.Invoke();
    }

    public static void Call_Start()
    {
        OnStart?.Invoke();
    }

    public static void Call_OnSlowpWheel()
    {
        OnSlowWheel?.Invoke();
    }

    public static void Call_OnStopWheel()
    {
        OnStopWheel?.Invoke();
    }

    public static void Call_OnScore(int score)
    {
        OnScore?.Invoke(score);
    }

}
