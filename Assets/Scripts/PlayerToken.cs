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
        }
        
        if (isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            trendState = PlayerTrendFollowState.All;
        }

        if (isTokenValidInFirstStreak && !isTokenValidInSecondStreak)
        {
            if (trendState == PlayerTrendFollowState.Trend2)
            {
                trendState = PlayerTrendFollowState.Trend1;   
                playerTokenList = new List<Token>();
            }
        }
        
        if (!isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            if (trendState == PlayerTrendFollowState.Trend1)
            {
                trendState = PlayerTrendFollowState.Trend2;   
                playerTokenList = new List<Token>();
            }
        }
        
        playerTokenList.Add(token);
        
        OnPlayerHitTokenPointUpdate?.Invoke(playerTokenList.Count);
    }

    private void GrabbedTheCorrectToken()
    {
        
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Token"))
        {
            Token token = other.gameObject.GetComponent<Token>();
            GrabTheToken(token.GetID(), token);
            Debug.Break();
        }
    }
    
    
}
