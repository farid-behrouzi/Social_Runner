using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] ScorePopup scorePopupPrefab = null;
    [SerializeField] float popupLifeTime = 1.0f;

    [SerializeField] private TextMeshProUGUI scoreTMP = null;
    [SerializeField] private TextMeshProUGUI addedTMP = null;
    [SerializeField] private StreakUI trendLeft = null;
    [SerializeField] private StreakUI trendRight = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private string fadeTriggerLabel = "ClearView";
    [SerializeField] private string addAmountLabel = "AddAmount";
    [SerializeField] private Transform debugPanel = null;


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
        if (scorePopupPrefab != null)
        {
            ScorePopup popup = Instantiate(scorePopupPrefab, _position, Quaternion.identity) as ScorePopup;
            popup.LifeTime = popupLifeTime;
            popup.Score = _score;
            popup.Init();
        }
    }

    private void PopAddedAmountScore(int _score)
    {
        if (addedTMP)
        {
            addedTMP.text = "+" + _score.ToString();
            if (animator)
            {
                animator.SetTrigger(addAmountLabel);
            }
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

    public void ClearTrend(TrendStreakType _type)
    {
        switch (_type)
        {
            case TrendStreakType.Type1:
            default:
                trendLeft.ClearStreak();
                break;
            case TrendStreakType.Type2:
                trendRight.ClearStreak();
                break;
        }
    }
    public void ResetTrend(TrendStreakType _type, bool _won = false)
    {
        switch (_type)
        {
            case TrendStreakType.Type1:
            default:
                trendLeft.Reset(_won);
                break;
            case TrendStreakType.Type2:
                trendRight.Reset(_won);
                break;
        }
    }

    public void Score(int totalScore,int addedAmount = 0)
    {
        if (scoreTMP)
        {
            scoreTMP.text = totalScore.ToString();
            EventManager.Call_OnPing("Number");
            if (addedAmount > 0)
            {
                PopAddedAmountScore(addedAmount);
            }
        }
    }

    public void FadeAway()
    {
        if (animator)
        {
            animator.SetTrigger(fadeTriggerLabel);
        }
    }
    #endregion

    #region Debug Methods

    private void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            if (debugPanel)
            {
                debugPanel.gameObject.SetActive(!debugPanel.gameObject.activeSelf);
            }
        }
    }

    public void SimulateNewTrend()
    {
        List<Light> lights = new();
        lights.Add(new Light() {color = Color.blue });
        lights.Add(new Light() {color = Color.green });
        lights.Add(new Light() {color = Color.red });
        CreatetTrend(TrendStreakType.Type1, lights);
        Score(50);
    }

    public void SimulateHit(int index)
    {
        TurnTrendLight(TrendStreakType.Type1, index);
        if (int.TryParse(scoreTMP.text, out int lastScore)) ;
            Score(lastScore + 10, 10 * (index + 1));
        if (index == 2)
        {
            ResetTrend(TrendStreakType.Type1, true);
        }
    }

    #endregion
}
