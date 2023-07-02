using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{

    private TrendStreakType playerTrendType;
    private List<Token> playerTokenList = new List<Token>();
    private PlayerTrendFollowState trendState;

    public static Action<int> OnPlayerHitTokenPointUpdate;
    


    public void GrabTheToken(int ID, Token token)
    {
        bool isTokenValidInFirstStreak = false;
        bool isTokenValidInSecondStreak = false;
        isTokenValidInFirstStreak = TokenTrend.instance.CheckThePlayerHitTokenInFirstStreak(ID, playerTokenList);
        isTokenValidInSecondStreak = TokenTrend.instance.CheckThePlayerHitTokenInSecondStreak(ID, playerTokenList);

        if (!isTokenValidInFirstStreak && !isTokenValidInSecondStreak)
        {
            trendState = PlayerTrendFollowState.None;
            playerTokenList = new List<Token>();
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type1);
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type2);
            return;
        }
        
        if (isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            trendState = PlayerTrendFollowState.All;
            GameEvents.PlayerHitToken(TrendStreakType.Type1, playerTokenList.Count);
            GameEvents.PlayerHitToken(TrendStreakType.Type2, playerTokenList.Count);
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type1, playerTokenList))
            {
                TrendCompleted();
                return;   
            }
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type2, playerTokenList))
            {
                TrendCompleted();
                return;  
            }
        }

        if (isTokenValidInFirstStreak && !isTokenValidInSecondStreak)
        {
            if (trendState == PlayerTrendFollowState.Trend2)
            {
                trendState = PlayerTrendFollowState.Trend1;   
                playerTokenList = new List<Token>();
            }
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type2);
            GameEvents.PlayerHitToken(TrendStreakType.Type1, playerTokenList.Count);
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type1, playerTokenList))
            {
                TrendCompleted();
                return;   
            }
        }
        
        if (!isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            if (trendState == PlayerTrendFollowState.Trend1)
            {
                trendState = PlayerTrendFollowState.Trend2;   
                playerTokenList = new List<Token>();
            }
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type1);
            GameEvents.PlayerHitToken(TrendStreakType.Type2, playerTokenList.Count);
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type2, playerTokenList))
            {
                TrendCompleted();
                return;   
            }
        }
        
        playerTokenList.Add(token);
        
        OnPlayerHitTokenPointUpdate?.Invoke(playerTokenList.Count);
    }

    private void TrendCompleted()
    {
        Debug.Log("TrendCompleted");
        playerTokenList = new List<Token>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Token"))
        {
            Token token = other.gameObject.GetComponent<Token>();
            GrabTheToken(token.GetID(), token);
            //Debug.Break();
        }
    }
    
    
}
