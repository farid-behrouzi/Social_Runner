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
        GameEvents.OnPlayerHitTokenUIUpdate += TurnTrendLight;
        GameEvents.OnCreateTrendUIUpdate += SetTrend;
        GameEvents.OnResetTrendUIUpdate += ResetTrend;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerHitTokenUIUpdate -= TurnTrendLight;
        GameEvents.OnCreateTrendUIUpdate -= SetTrend;
        GameEvents.OnResetTrendUIUpdate -= ResetTrend;
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

    /// <summary>
    /// pings the UI element on the scene visually if the label matches the pingable label
    /// </summary>
    /// <param name="label"></param>
    public void Ping(string label)
    {
        EventManager.Call_OnPing(label);
    }

    /// <summary>
    /// instantiate new trend lights UI elements based on given list of Lights
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_lights"></param>
    public void SetTrend(TrendStreakType _type, List<Light> _lights)
    {
        ClearTrend(_type);
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

    /// <summary>
    /// highlights the lights with given id as index of the list
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_id"> index of the light in the list. 
    /// if _id is 0 the first light will be highlighted and if 1 the second one ...</param>
    public void TurnTrendLight(TrendStreakType _type, int _id)
    {
        Debug.Log("F@rid=> id: " + _id);
        Debug.Log("TurnTrendLight");
        
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

    /// <summary>
    /// destroys the trend UI childs
    /// </summary>
    /// <param name="_type"></param>
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

    /// <summary>
    /// runs UI feedback based on the result(_won) of the streak
    /// to clear the trend ui childs directly use ClearTrend(_type)
    /// trend childs will be cleared automatically by setting a new trend
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_won">if true, trend UI gives a positive feedback and negative in case of fail</param>
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

    /// <summary>
    /// updates UI score number and runs a popup to show the added amount
    /// </summary>
    /// <param name="totalScore">number to be shown as player's total score</param>
    /// <param name="addedAmount">new added amount that has made the current total number</param>
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

    /// <summary>
    /// runs in game menu UI wipe out animation 
    /// </summary>
    public void FadeUIAway()
    {
        if (animator)
        {
            animator.SetTrigger(fadeTriggerLabel);
        }
    }

    /// <summary>
    /// updates the UI with the last streak feedback result and new set of lights for the next one
    /// </summary>
    /// <param name="_type">type1 : left trend panel, type2: right trend panel</param>
    /// <param name="_OldResult">if True, UI gives a successful feedback regarding to the last streak</param>
    /// <param name="_NewLights">new set of Light objects to create a new trend UI elements accordingly</param>
    public void FinishOldStartNew(TrendStreakType _type, bool _OldResult, List<Light> _NewLights)
    {
        ResetTrend(_type, _OldResult);
        SetTrend(_type, _NewLights);
    }
    #endregion

    #region Debug Methods

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
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
        SetTrend(TrendStreakType.Type1, lights);
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
            SimulateSetNextNewTrend();
        }
    }

    public void SimulateSetNextNewTrend()
    {
        List<Light> lights = new();
        lights.Add(new Light() { color = Color.yellow });
        lights.Add(new Light() { color = Color.cyan });
        lights.Add(new Light() { color = Color.magenta });
        FinishOldStartNew(TrendStreakType.Type1, true, lights);
        Score(130);
    }

    #endregion


}
