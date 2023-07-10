using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
    public static Action OnWheelStopped;
    public static Action<int, PlayerTrendFollowState> OnUpdateCurrentTokenInTrend;
    public static Action OnPlayerHitSideCollider;

    public static Token token;

    private static List<Token> firstTrendStreakList;
    private static List<Token> secondTrendStreakList;
    private static PlayerTrendFollowState playerTrendFollowState;


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

    public static void Call_OnplayerBadgeUpUIUpdate(int level, int points)
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

    public static void Call_OnUpdateCurrentTokenInTrend(int currentTokenCounter, PlayerTrendFollowState playerTrendFollowState)
    {
        OnUpdateCurrentTokenInTrend?.Invoke(currentTokenCounter, playerTrendFollowState);
    }


    public static void SetFirstTrendStreak(List<Token> _firstTrendStreakList) 
    {
        firstTrendStreakList = new List<Token>();
        firstTrendStreakList = _firstTrendStreakList;
    }
    
    public static void SetSecondTrendStreak(List<Token> _secondTrendStreakList) 
    {
        secondTrendStreakList = new List<Token>();
        secondTrendStreakList = _secondTrendStreakList;
    }

    public static List<Token> GetCurrentTrendStreakList()
    {
        switch (playerTrendFollowState)
        {
            case PlayerTrendFollowState.Trend1:
                return firstTrendStreakList;
            case PlayerTrendFollowState.Trend2:
                return secondTrendStreakList;
            case PlayerTrendFollowState.All:
                return GetCombinationOfTwiTrendsList();
            case PlayerTrendFollowState.None:
                return GetCombinationOfTwiTrendsList();
            default:
                return GetCombinationOfTwiTrendsList();
        }
    }

    public static void SetPlayerTrendFollowState(PlayerTrendFollowState _playerTrendFollowState)
    {
        playerTrendFollowState = _playerTrendFollowState;
    }

    private static List<Token> GetCombinationOfTwiTrendsList()
    {
        List<Token> combinationList = new List<Token>();
        foreach (Token token in firstTrendStreakList)
        {
            combinationList.Add(token);
        }
        foreach (Token token in secondTrendStreakList)
        {
            combinationList.Add(token);
        }
        combinationList = combinationList.Distinct().ToList();
        return combinationList;
    }

    public static void Call_OnPlayerHitSideCollider()
    {
        OnPlayerHitSideCollider?.Invoke();
    }

    public static void Call_OnWheelStopped()
    {
        OnWheelStopped?.Invoke();
    }

}
