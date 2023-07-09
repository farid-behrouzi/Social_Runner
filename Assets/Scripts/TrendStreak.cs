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
    private float secondCounter;

    private bool justGenerated = true;


    private void Start()
    {
        switch (type)
        {
            case TrendStreakType.Type1:
                tokenTrend.OnFirstTokenStreakCompleted += Terminate;
                break;
            case TrendStreakType.Type2:
                tokenTrend.OnSecondTokenStreakCompleted += Terminate;
                break;
        }

        InvokeRepeating(nameof(ReductAttentionPoint), 0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            return;
        }

        GameEvents.Call_OnRivalSnapshopt();
        Terminate();
    }

    private void ReductAttentionPoint()
    {
        secondCounter++;
        GameEvents.ReductAttentionPoint(secondCounter);
    }

    public void SetLifeTime(float lifeTime)
    {
        this.lifeTime = lifeTime;
    }

    public void SetTokenType(TrendStreakType type)
    {
        this.type = type;
    }

    public void SetTokenTrend(TokenTrend tokenTrend)
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
            Light newLight = new Light(){id = token.GetID(), color = token.GetColor(tokenTrend)};
            uiLightsList.Add(newLight);
        }
        GameEvents.CreateTrendUIUpdate(type, uiLightsList);
    }

    private void DistroyItself()
    {
        //CancelInvoke(nameof(ReductAttentionPoint));
        //DestroyImmediate(gameObject);
    }

    private void Terminate()
    {
        CancelInvoke(nameof(ReductAttentionPoint));
        switch (type)
        {
            case TrendStreakType.Type1:
                tokenTrend.OnFirstTokenStreakCompleted -= Terminate;
                tokenTrend.GenerateFirstTokenStreak();
                break;
            case TrendStreakType.Type2:
                tokenTrend.OnSecondTokenStreakCompleted -= Terminate;
                tokenTrend.GenerateSecondTokenStreak();
                break;
        }
        
        Destroy(gameObject);
    }
}
