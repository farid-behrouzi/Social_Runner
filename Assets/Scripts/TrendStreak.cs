using UnityEngine;

public class TrendStreak : MonoBehaviour
{

    private float timer;
    private float lifeTime;
    private TokenTrend tokenTrend;
    private TrendStreakType type;

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


    private void OnDestroy()
    {
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
