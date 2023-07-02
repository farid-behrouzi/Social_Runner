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
    [SerializeField] private Animator animator = null;
    [SerializeField] private string fadeTriggerLabel = "ClearView";


    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        
    }

    private void Awake()
    {
        if (animator == null && !this.gameObject.TryGetComponent(out animator))
        {
            Debug.LogError("no animator found in UI manager");
        }
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

    #region Public Methods

    public void Ping(string label)
    {
        EventManager.Call_OnPing(label);
    }

    public void CreatetTrend(TrendStreakType _type, List<Light> _lights)
    {
        switch (_type)
        {
            case TrendStreakType.Type1:
            default:
                trendLeft.CreateNewStreak(_lights);
                break;
            case TrendStreakType.Type2:
                trendRight.CreateNewStreak(_lights);
                break;
        }
    }

    public void TurnTrendLight(TrendStreakType _type, int _id)
    {
        switch (_type)
        {
            case TrendStreakType.Type1:
            default:
                trendLeft.TurnLight(_id);
                break;
            case TrendStreakType.Type2:
                trendRight.TurnLight(_id);
                break;
        }
        
    }

    public void Score(int _rewardAmount)
    {

    }

    public void FadeAway()
    {
        if (animator)
        {
            animator.SetTrigger(fadeTriggerLabel);
        }
    }
    #endregion
}
