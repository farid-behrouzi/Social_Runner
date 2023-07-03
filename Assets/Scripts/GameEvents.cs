using System;
using System.Collections;
using System.Collections.Generic;

public class GameEvents
{

    public static Action<TrendStreakType, int> OnPlayerHitTokenUIUpdate;
    public static Action<TrendStreakType, List<Light>> OnCreateTrendUIUpdate;
    public static Action<TrendStreakType, bool> OnResetTrendUIUpdate;
    

    public static void PlayerHitToken(TrendStreakType trendType, int tokenID)
    {
        OnPlayerHitTokenUIUpdate?.Invoke(trendType, tokenID);
    }

    public static void CreateTrendUIUpdate(TrendStreakType trendType, List<Light> lights)
    {
        OnCreateTrendUIUpdate?.Invoke(trendType, lights);
    }

    public static void ResetTrendUIUpdate(TrendStreakType trendType)
    {
        OnResetTrendUIUpdate?.Invoke(trendType, false);
    }

    public static void Call_OnFinishStreak(TrendStreakType trendType, bool result)
    {
        OnResetTrendUIUpdate?.Invoke(trendType, result);
    }

    public static void Call_OnNewTrend(TrendStreakType trendType, List<Light> lights)
    {
        OnCreateTrendUIUpdate?.Invoke(trendType, lights);
    }

}
