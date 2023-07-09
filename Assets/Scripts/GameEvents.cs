using System;
using System.Collections;
using System.Collections.Generic;

public class GameEvents
{

    public static Action<TrendStreakType, int> OnPlayerHitTokenUIUpdate;
    public static Action<TrendStreakType, List<Light>> OnCreateTrendUIUpdate;
    public static Action<TrendStreakType> OnResetTrendUIUpdate;
    public static Action<int, int> OnPlayerScore;
    public static Action<float> OnPointReduction;
    public static Action<int> OnPlayerCompletedTrend;
    public static Action<int, int> OnPlayerLevelUpUIUpdate;
    public static Action OnTrendIsGone;
    public static Action OnStopWheelSmoothly;
    public static Action OnCancelWheelReduction;


    public static void PlayerHitToken(TrendStreakType trendType, int tokenID)
    {
        OnPlayerHitTokenUIUpdate?.Invoke(trendType, tokenID);
    }

    public static void PlayerScoreUIUpdate(int totalScore, int newPoint)
    {
        OnPlayerScore?.Invoke(totalScore, newPoint);
    }

    public static void CreateTrendUIUpdate(TrendStreakType trendType, List<Light> lights)
    {
        OnCreateTrendUIUpdate?.Invoke(trendType, lights);
    }

    public static void ResetTrendUIUpdate(TrendStreakType trendType)
    {
        OnResetTrendUIUpdate?.Invoke(trendType);
    }

    public static void ReductAttentionPoint(float reductionValue)
    {
        OnPointReduction?.Invoke(reductionValue);
    }

    public static void Call_OnPlayerCompletedTrend(int playerPoint)
    {
        OnPlayerCompletedTrend?.Invoke(playerPoint);
    }

    public static void Call_OnPlayerLevelUpUIUpdate(int level, int points)
    {
        OnPlayerLevelUpUIUpdate?.Invoke(level, points);
    }
    
    public static void Call_OnRivalSnapshopt()
    {
        OnTrendIsGone?.Invoke();
    }

    public static void Call_OnFinishStreak(TrendStreakType trendType, bool result)
    {
        OnResetTrendUIUpdate?.Invoke(trendType);
    }

    public static void Call_OnNewTrend(TrendStreakType trendType, List<Light> lights)
    {
        OnCreateTrendUIUpdate?.Invoke(trendType, lights);
    }

    public static void Call_OnStopWheelSmoothly()
    {
        OnStopWheelSmoothly?.Invoke();
    }
    
    public static void Call_OnCancelWheelSpeedReduction()
    {
        OnCancelWheelReduction?.Invoke();
    }

}
