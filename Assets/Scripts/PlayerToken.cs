using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{

    private TrendStreakType playerTrendType;
    private List<Token> playerFirstTrendTokenList = new List<Token>();
    private List<Token> playerSecondTrendTokenList = new List<Token>();
    private PlayerTrendFollowState trendState;
    
    private int playersFirstTokenCounter;
    private int playersSecondTokenCounter;

    private PlayerScore playerScore;


    private void Awake()
    {
        playerScore = GetComponent<PlayerScore>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("Start");
        UpdateCurrentTokenToGet(playersFirstTokenCounter > playersSecondTokenCounter ? playersFirstTokenCounter : playersSecondTokenCounter, trendState);
    }

    public void GrabTheToken(int ID, Token token)
    {
        bool isTokenValidInFirstStreak = false;
        bool isTokenValidInSecondStreak = false;
        isTokenValidInFirstStreak = TokenTrend.instance.CheckThePlayerHitTokenInFirstStreak(ID, playerFirstTrendTokenList);
        isTokenValidInSecondStreak = TokenTrend.instance.CheckThePlayerHitTokenInSecondStreak(ID, playerSecondTrendTokenList);

        if (!isTokenValidInFirstStreak && !isTokenValidInSecondStreak)
        {
            playersFirstTokenCounter = 0;
            playersSecondTokenCounter = 0;
            trendState = PlayerTrendFollowState.None;
            playerFirstTrendTokenList = new List<Token>();
            playerSecondTrendTokenList = new List<Token>();
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type1);
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type2);
            return;
        }
        
        if (isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            playerScore.UpdatePoint();
            playerScore.UpdatePoint();
            trendState = PlayerTrendFollowState.All;
            GameEvents.PlayerHitToken(TrendStreakType.Type1, playersFirstTokenCounter);
            GameEvents.PlayerHitToken(TrendStreakType.Type2, playersSecondTokenCounter);
            playersFirstTokenCounter++;
            playersSecondTokenCounter++;
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type1, playerFirstTrendTokenList))
            {
                TrendCompleted(TrendStreakType.Type1);
                return;   
            }
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type2, playerSecondTrendTokenList))
            {
                TrendCompleted(TrendStreakType.Type2);
                return;  
            }
        }

        if (isTokenValidInFirstStreak && !isTokenValidInSecondStreak)
        {
            if (trendState == PlayerTrendFollowState.Trend2 || trendState == PlayerTrendFollowState.None || trendState == PlayerTrendFollowState.All)
            {
                trendState = PlayerTrendFollowState.Trend1;   
                playerSecondTrendTokenList = new List<Token>();
            }

            playersSecondTokenCounter = 0;
            playerScore.UpdatePoint();
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type2);
            GameEvents.PlayerHitToken(TrendStreakType.Type1, playersFirstTokenCounter);
            playersFirstTokenCounter++;
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type1, playerFirstTrendTokenList))
            {
                TrendCompleted(TrendStreakType.Type1);
                return;   
            }
        }
        
        if (!isTokenValidInFirstStreak && isTokenValidInSecondStreak)
        {
            if (trendState == PlayerTrendFollowState.Trend1 || trendState == PlayerTrendFollowState.None || trendState == PlayerTrendFollowState.All)
            {
                trendState = PlayerTrendFollowState.Trend2;   
                playerFirstTrendTokenList = new List<Token>();
            }

            playersFirstTokenCounter = 0;
            playerScore.UpdatePoint();
            GameEvents.ResetTrendUIUpdate(TrendStreakType.Type1);
            GameEvents.PlayerHitToken(TrendStreakType.Type2, playersSecondTokenCounter);
            playersSecondTokenCounter++;
            if (TokenTrend.instance.IsTrendCompleted(TrendStreakType.Type2, playerSecondTrendTokenList))
            {
                TrendCompleted(TrendStreakType.Type2);
                return;   
            }
        }

        //UpdateCurrentTokenToGet(playersFirstTokenCounter >= playersSecondTokenCounter ? playersFirstTokenCounter : playersSecondTokenCounter, trendState);
        GameEvents.SetPlayerTrendFollowState(trendState);
        switch (trendState)
        {
            case PlayerTrendFollowState.Trend1:
                playerFirstTrendTokenList.Add(token);
                break;
            case PlayerTrendFollowState.Trend2:
                playerSecondTrendTokenList.Add(token);
                break;
            case PlayerTrendFollowState.All:
                playerFirstTrendTokenList.Add(token);
                playerSecondTrendTokenList.Add(token);
                break;
        }
        //playerFirstTrendTokenList.Add(token);
    }

    private void UpdateCurrentTokenToGet(int currentTokenCounter, PlayerTrendFollowState playerTrendFollowState)
    {
        switch (playerTrendFollowState)
        {
            case PlayerTrendFollowState.Trend1:
                GameEvents.Call_OnUpdateCurrentTokenInTrend(currentTokenCounter, PlayerTrendFollowState.Trend1);
                break;
            case PlayerTrendFollowState.Trend2:
                GameEvents.Call_OnUpdateCurrentTokenInTrend(currentTokenCounter, PlayerTrendFollowState.Trend2);
                break;
            default:
                GameEvents.Call_OnUpdateCurrentTokenInTrend(currentTokenCounter, PlayerTrendFollowState.Trend1);
                break;
        }
    }

    private void TrendCompleted(TrendStreakType type)
    {
        switch (type)
        {
            case TrendStreakType.Type1:
                playerScore.UpdatePointOnTrendCompletion(playersFirstTokenCounter);
                playersFirstTokenCounter = 0;
                playerFirstTrendTokenList = new List<Token>();
                break;
            case TrendStreakType.Type2:
                playerScore.UpdatePointOnTrendCompletion(playersSecondTokenCounter);
                playersSecondTokenCounter = 0;
                playerSecondTrendTokenList = new List<Token>();
                break;  
        }
        trendState = PlayerTrendFollowState.None;  
        GameEvents.SetPlayerTrendFollowState(trendState);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Token"))
        {
            Token token = other.gameObject.GetComponent<Token>();
            GrabTheToken(token.GetID(), token);
            EventManager.Call_OnHit(token);
            //Debug.Break();
        }
    }
}
