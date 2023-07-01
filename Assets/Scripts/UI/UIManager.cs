using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] ScorePopup scorePopupPrefab = null;
    [SerializeField] float popupLifeTime = 1.0f;

    [SerializeField] private TextMeshProUGUI scoreTMP = null;
    [SerializeField] private StreakUI trendLeft = null;
    [SerializeField] private StreakUI trendRight = null;


    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void Awake()
    {

    }

    private void PopScore(int _score, Vector3 _position)
    {
        if(scorePopupPrefab != null)
        {
            ScorePopup popup = Instantiate(scorePopupPrefab, _position, Quaternion.identity) as ScorePopup;
            popup.LifeTime = popupLifeTime;
            popup.Score = _score;
            popup.Init();
        }
    }

    #region Test
    /// ----------------- TEST -----------------
    public void Ping(string label)
    {
        EventManager.Call_OnPing(label);
    }

    public void CreateDummyLeftTrend()
    {
        List<Light> newLights = new();
        for ( int i = 0; i < 3; i++)
        {
            Light l = new();
            switch (i)
            {
                case 0:
                    l.color = Color.green;
                    break;
                case 1:
                    l.color = Color.red;
                    break;
                case 2:
                    l.color = Color.blue;
                    break;
                default:
                    break;
            }
            newLights.Add(l);
        }
        trendLeft.CreateNewStreak(newLights);
    }

    public void TurnLeftTrendLight(int _id)
    {
        trendLeft.TurnLight(_id);
    }
    #endregion
}
