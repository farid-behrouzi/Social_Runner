using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{

    private TrendStreakType playerTrendType;
    private List<Token> playerTokenList = new List<Token>();
    private PlayerTrendFollowState trendState;

    public static Action<int> OnPlayerHitTokenPointUpdate;
    private int playersFirstTokenCounter;
    private int playersSecondTokenCounter;
    


    public void GrabTheToken(int ID, Token token)
    {
        bool isTokenValidInFirstStreak = false;
        bool isTokenValidInSecondStreak = false;
        isTokenValidInFirstStreak = TokenTrend.instance.CheckThePlayerHitTokenInFirstStreak(ID, playerTokenList);
        isTokenValidInSecondStreak = TokenTrend.instance.CheckThePlayerHitTokenInSecondStreak(ID, playerTokenList);

        if (!isTokenValidInFirstStreak && !isTokenValidInSecondStreak)
        {
            playersFirstTokenCounter = 0;
            playersSecondTokenCounter = 0;
            trendState = PlayerTrendFollowState.None;
            playerTokenList = new List<Token>();
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type1);
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type2);
            return;
        }
        
        if (isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            playersFirstTokenCounter++;
            playersSecondTokenCounter++;
            OnPlayerHitTokenPointUpdate?.Invoke(playersFirstTokenCounter);
            OnPlayerHitTokenPointUpdate?.Invoke(playersSecondTokenCounter);
            trendState = PlayerTrendFollowState.All;
            GameEvents.PlayerHitToken(TrendStreakType.Type1, playerTokenList.Count);
            GameEvents.PlayerHitToken(TrendStreakType.Type2, playerTokenList.Count);
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type1, playerTokenList))
            {
                TrendCompleted(TrendStreakType.Type1);
                return;   
            }
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type2, playerTokenList))
            {
                TrendCompleted(TrendStreakType.Type2);
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

            playersSecondTokenCounter = 0;
            playersFirstTokenCounter++;
            OnPlayerHitTokenPointUpdate?.Invoke(playersFirstTokenCounter);
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type2);
            GameEvents.PlayerHitToken(TrendStreakType.Type1, playerTokenList.Count);
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type1, playerTokenList))
            {
                TrendCompleted(TrendStreakType.Type1);
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

            playersFirstTokenCounter = 0;
            playersSecondTokenCounter++;
            OnPlayerHitTokenPointUpdate?.Invoke(playersSecondTokenCounter);
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type1);
            GameEvents.PlayerHitToken(TrendStreakType.Type2, playerTokenList.Count);
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type2, playerTokenList))
            {
                TrendCompleted(TrendStreakType.Type2);
                return;   
            }
        }
        
        playerTokenList.Add(token);
        //OnPlayerHitTokenPointUpdate?.Invoke(playerTokenList.Count);
    }

    private void TrendCompleted(TrendStreakType type)
    {
        switch (type)
        {
            case TrendStreakType.Type1:
                playersFirstTokenCounter = 0;
                break;
            case TrendStreakType.Type2:
                playersSecondTokenCounter = 0;
                break;
        }
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
