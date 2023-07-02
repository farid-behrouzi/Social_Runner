using System;
using System.Collections.Generic;
using UnityEngine;

public class TrendStreak : MonoBehaviour
{

    private float timer;
    private float lifeTime;
    private TokenTrend tokenTrend;
    private TrendStreakType type;
    private List<Token> tokenList = new List<Token>();
    private List<Light> uiLightsList = new List<Light>();


    private void Start()
    {
        switch (type)
        {
            case TrendStreakType.Type1:
                tokenTrend.OnFirstTokenStreakCompleted += DistroyItself;
                break;
            case TrendStreakType.Type2:
                tokenTrend.OnSecondTokenStreakCompleted += DistroyItself;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            return;
        }
        
        Destroy(gameObject);
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    public void SetTokenType(TrendStreakType type)
    {
        this.type = type;
    }

    public void SetTokeTrend(TokenTrend tokenTrend)
    {
        this.tokenTrend = tokenTrend;
    }

    public void SetTokenList(List<Token> tokenList)
    {
        this.tokenList = tokenList;

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Token token in tokenList)
        {
            Light newLight = new Light(){id = token.GetID(), color = token.GetColor()};
            uiLightsList.Add(newLight);
        }
        
        Debug.Log("UpdateUI");
        GameEvents.CreateTrendUIUpdate(type, uiLightsList);
    }

    private void DistroyItself()
    {
        Destroy(gameObject);
    }


    private void OnDestroy()
    {
        tokenTrend.OnFirstTokenStreakCompleted -= DistroyItself;
        tokenTrend.OnSecondTokenStreakCompleted -= DistroyItself;
        
        switch (type)
        {
            case TrendStreakType.Type1:
                tokenTrend.GenerateFirstTokenStreak();
                break;
            case TrendStreakType.Type2:
                tokenTrend.GenerateSecondTokenStreak();
                break;
        }
    }
}
