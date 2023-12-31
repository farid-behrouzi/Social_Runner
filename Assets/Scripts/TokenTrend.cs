using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TokenTrend : MonoBehaviour
{

    public static TokenTrend instance;
    
    [SerializeField] private Transform[] tokenArray;

    [SerializeField] private int streakLengthMinimum;
    [SerializeField] private int streakLengthMaximum;

    private List<Token> firstTrendStreakList;
    private List<Token> secondTrendStreakList;

    [SerializeField] private float firstTrendStreakLifeTimeMinimum;
    [SerializeField] private float firstTrendStreakLifeTimeMaximum;
    [SerializeField] private float secondTrendStreakLifeTimeMinimum;
    [SerializeField] private float secondTrendStreakLifeTimeMaximum;
    
    
    private float firstTrendStreakLifeTime;
    private float secondTrendStreakLifeTime;
    
    private float firstTrendStreakTimer;
    private float secondTrendStreakTimer;

    private TrendStreak firstTrendStreak;
    private TrendStreak secondTrendStreak;


    public Action OnFirstTokenStreakCompleted;
    public Action OnSecondTokenStreakCompleted;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        GenerateFirstTokenStreak();
        GenerateSecondTokenStreak();

        //GameEvents.OnWheelStopped += DisableTrendStreaks;
        //GameEvents.OnUpdateCurrentTokenInTrend += UpdateCurrentTokenToGet;
    }

    private void OnDisable()
    {
        //GameEvents.OnWheelStopped -= DisableTrendStreaks;
    }

    private int GetRandomLengthForTrendStreak()
    {
        return Random.Range(streakLengthMinimum, streakLengthMaximum);
    }

    private Token GetRandomTokenForTrendStreak()
    {
        return tokenArray[Random.Range(0, tokenArray.Length)].GetComponent<Token>();
    }

    private void DisableTrendStreaks()
    {
        firstTrendStreak.enabled = false;
        secondTrendStreak.enabled = false;
    }

    public void GenerateFirstTokenStreak()
    {
        SetTrendStreaksLifeTime();
        firstTrendStreakList = new List<Token>();
        GenerateTokenStreak(firstTrendStreakList);
        GameObject trendStreakObject = new GameObject("FirstTrendStreak");
        TrendStreak trendStreak = trendStreakObject.AddComponent<TrendStreak>();
        trendStreak.SetLifeTime(firstTrendStreakLifeTime);
        trendStreak.SetTokenType(TrendStreakType.Type1);
        trendStreak.SetTokenTrend(this);
        trendStreak.SetTokenList(firstTrendStreakList);
    }
    
    public void GenerateSecondTokenStreak()
    {
        SetTrendStreaksLifeTime();
        secondTrendStreakList = new List<Token>();
        GenerateTokenStreak(secondTrendStreakList);
        GameObject trendStreakObject = new GameObject("SecondTrendStreak");
        TrendStreak trendStreak = trendStreakObject.AddComponent<TrendStreak>();
        trendStreak.SetLifeTime(secondTrendStreakLifeTime);
        trendStreak.SetTokenType(TrendStreakType.Type2);
        trendStreak.SetTokenTrend(this);
        trendStreak.SetTokenList(secondTrendStreakList);
    }
    
    private void GenerateTokenStreak(List<Token> tokenStreakList)
    {
        int trendStreakLength = GetRandomLengthForTrendStreak();

        for (int i = 0; i < trendStreakLength; i++)
        {
            tokenStreakList.Add(GetRandomTokenForTrendStreak());
        }
    }

    private void SetTrendStreaksLifeTime()
    {
        firstTrendStreakLifeTime = Random.Range(firstTrendStreakLifeTimeMinimum, firstTrendStreakLifeTimeMaximum);
        secondTrendStreakLifeTime = Random.Range(secondTrendStreakLifeTimeMinimum, secondTrendStreakLifeTimeMaximum);
    }

    public List<Token> WhatIsTheFirstToken(int ID)
    {
        if (firstTrendStreakList[0].GetID() == ID)
        {
            return firstTrendStreakList;
        }
        if (secondTrendStreakList[0].GetID() == ID)
        {
            return secondTrendStreakList;
        }
        return null;
    }
    
    public bool IsTokenValid(TrendStreakType type, int ID, int index)
    {
        List<Token> toLookList = new List<Token>();

        if (type == TrendStreakType.None)
        {
            return false;
        }
        
        switch (type)
        {
            case TrendStreakType.Type1:
                return IsTokenIDValid(ID, index, firstTrendStreakList);
            case TrendStreakType.Type2:
                return IsTokenIDValid(ID, index, secondTrendStreakList);
        }

        return false;
    }

    private bool IsTokenIDValid(int ID, int index, List<Token> toLookList)
    {
        return toLookList[index].GetID() == ID;
    }

    public void SetTheSpawnedTokenType(Token token)
    {
        foreach (Token item in firstTrendStreakList)
        {
            if (item.GetID() == token.GetID())
            {
                token.SetTokenType(TrendStreakType.Type1);
                return;
            }
        }
        foreach (Token item in secondTrendStreakList)
        {
            if (item.GetID() == token.GetID())
            {
                token.SetTokenType(TrendStreakType.Type2);
                return;
            }
        }
        token.SetTokenType(TrendStreakType.None);
    }

    public bool CheckThePlayerHitTokenInFirstStreak(int tokenID, List<Token> playerTokenList)
    {

        for (int i = 0; i < playerTokenList.Count; i++)
        {
            if (playerTokenList[i].GetID() != firstTrendStreakList[i].GetID())
            {
                return false;
            }
        }

        return firstTrendStreakList[playerTokenList.Count].GetID() == tokenID;
    }
    
    public bool CheckThePlayerHitTokenInSecondStreak(int tokenID, List<Token> playerTokenList)
    {
        for (int i = 0; i < playerTokenList.Count; i++)
        {
            if (playerTokenList[i].GetID() != secondTrendStreakList[i].GetID())
            {
                return false;
            }
        }

        return secondTrendStreakList[playerTokenList.Count].GetID() == tokenID;
    }

    public bool IsTrendCompleted(TrendStreakType trendType, List<Token> playerTokenList)
    {

        switch (trendType)
        {
            case TrendStreakType.Type1:
                if (firstTrendStreakList.Count == playerTokenList.Count + 1)
                {
                    OnFirstTokenStreakCompleted?.Invoke();
                    return true;
                }
                break;
            case TrendStreakType.Type2:
                if (secondTrendStreakList.Count == playerTokenList.Count + 1)
                {
                    OnSecondTokenStreakCompleted?.Invoke();
                    return true;
                }
                break;
        }

        return false;
    }

    public Color GetTokenColor(int ID)
    {
        foreach (Transform token in tokenArray)
        {
            if (token.GetComponent<Token>().GetID() == ID)
            {
                return token.GetComponent<MeshRenderer>().sharedMaterial.color;
            }
        }

        return Color.black;
    }
    
    private void UpdateCurrentTokenToGet(int currentTokenCounter, PlayerTrendFollowState playerTrendFollowState)
    {
        Debug.Log("UpdateCurrentTokenToGet");
        switch (playerTrendFollowState)
        {
            case PlayerTrendFollowState.Trend1:
                GameEvents.token = firstTrendStreakList[currentTokenCounter];
                break;
            case PlayerTrendFollowState.Trend2:
                GameEvents.token = secondTrendStreakList[currentTokenCounter];
                break;
            default:
                GameEvents.token = secondTrendStreakList[currentTokenCounter];
                break;
        }
        
        Debug.Log(GameEvents.token.ToString());
    }
    
}
