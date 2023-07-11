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

    private float mappedLifeTime;


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

    private void OnEnable()
    {
        GameEvents.OnWheelStopped += Disable;
    }

    private void OnDisable()
    {
        GameEvents.OnWheelStopped -= Disable;
    }

    private void Disable()
    {
        enabled = false;
    }



    // Update is called once per frame
    void Update()
    {
        if (lifeTime == 0)
        {
            return;
        }
        while (timer < lifeTime)
        {
            timer += Time.deltaTime;
            mappedLifeTime = Remap(timer, 0f, lifeTime, 0f, 1f);
            SetCurrentLifeTimeInGameEvents(mappedLifeTime);
            return;
        }
        
        GameEvents.Call_OnRivalSnapshopt();
        Terminate();
    }

    private void SetCurrentLifeTimeInGameEvents(float lifetime)
    {
        switch (type)
        {
            case TrendStreakType.Type1:
                GameEvents.firstTrendLifeTime = lifetime;
                break;
            case TrendStreakType.Type2:
                GameEvents.secondTrendLifeTime = lifetime;
                break;
        }
    }
    
    float Remap(float value, float originalMin, float originalMax, float newMin, float newMax)
    {
        // Clamp the value between the original range
        value = Mathf.Clamp(value, originalMin, originalMax);

        // Calculate the normalized value (between 0 and 1) in the original range
        float normalizedValue = (value - originalMin) / (originalMax - originalMin);

        // Lerp the normalized value to the new range
        float remappedValue = Mathf.Lerp(newMin, newMax, normalizedValue);

        return remappedValue;
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

        switch (type)
        {
            case TrendStreakType.Type1:
                GameEvents.SetFirstTrendStreak(tokenList);
                break;
            case TrendStreakType.Type2:
                GameEvents.SetSecondTrendStreak(tokenList);
                break;
        }
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
