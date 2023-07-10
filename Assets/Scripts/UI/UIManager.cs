using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

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
    [SerializeField] private Canvas startScreen = null;
    [SerializeField] private Snapshot playerSnapshotUI = null;
    [SerializeField] private Snapshot rivalSnapshotUI = null;
    [SerializeField] private UnityEngine.UI.Image badgeImage = null;

    [SerializeField] private List<Sprite> playerPhotos = new();
    [SerializeField] private List<Sprite> rivalPhotos = new();

    [SerializeField] private TextMeshProUGUI levelTmp = null;
    [SerializeField] private PlayableDirector director = null;
    public AudioPlayer audioPlayer = null;


    private void OnEnable()
    {
        GameEvents.OnPlayerHitTokenUIUpdate += TurnTrendLight;
        GameEvents.OnCreateTrendUIUpdate += SetTrend;
        GameEvents.OnResetTrendUIUpdate += ResetTrendTokens;
        GameEvents.OnPlayerScore += Score;
        GameEvents.OnPlayerCompletedTrend += TakePlayerSnapshot;
        GameEvents.OnPlayerLevelUpUIUpdate += LevelUpUI;
        GameEvents.OnTrendIsGone += LeftBehindUI;
    }
    private void OnDisable()
    {
        GameEvents.OnPlayerHitTokenUIUpdate -= TurnTrendLight;
        GameEvents.OnCreateTrendUIUpdate -= SetTrend;
        GameEvents.OnResetTrendUIUpdate -= ResetTrendTokens;
        GameEvents.OnPlayerScore -= Score;
        GameEvents.OnPlayerCompletedTrend -= TakePlayerSnapshot;
        GameEvents.OnPlayerLevelUpUIUpdate -= LevelUpUI;
        GameEvents.OnTrendIsGone -= LeftBehindUI;
    }

    private void Awake()
    {
        if (animator == null && !this.gameObject.TryGetComponent(out animator))
        {
            Debug.LogError("no animator found in UI manager");
        }
        if (startScreen == null)
        {
            Debug.LogWarning("no start screen found");
        }
        if (audioPlayer == null && !this.gameObject.TryGetComponent(out audioPlayer))
        {
            Debug.LogError("no audio source found in UI manager");
        }
    }

    private void Start()
    {
        if (startScreen)
        {
            startScreen.enabled = true;
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
            addedTMP.text = (_score > 0 ? "+" : "") + _score.ToString();
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
        //ClearTrend(_type);
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

        EventManager.Call_OnHit(null);
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
        Debug.Log("ResetTrend");
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
    /// turns off all lights in trend UI with given type
    /// </summary>
    /// <param name="_type"></param>
    public void ResetTrendTokens(TrendStreakType _type)
    {
        switch (_type)
        {
            case TrendStreakType.Type1:
            default:
                trendLeft.ResetTokens();
                break;
            case TrendStreakType.Type2:
                trendRight.ResetTokens();
                break;
        }
    }

    /// <summary>
    /// updates UI score number and runs a popup to show the added amount
    /// </summary>
    /// <param name="totalScore">number to be shown as player's total score</param>
    /// <param name="addedAmount">new added amount that has made the current total number</param>
    private void Score(int totalScore,int addedAmount = 0)
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

    public void RunEndingCutScene()
    {
        if (director)
        {
            director.Play();
        }
        EventManager.Call_End();
    }


    /// <summary>
    /// updates the UI with the last streak feedback result and new set of lights for the next one
    /// </summary>
    /// <param name="_type">type1 : left trend panel, type2: right trend panel</param>
    /// <param name="_OldResult">if True, UI gives a successful feedback regarding to the last streak</param>
    /// <param name="_NewLights">new set of Light objects to create a new trend UI elements accordingly</param>
    public void FinishOldStartNew(TrendStreakType _type, bool _OldResult, List<Light> _NewLights, int _points = 0)
    {
        ResetTrend(_type, _OldResult);
        SetTrend(_type, _NewLights);
        if (_OldResult)
        {
            TakePlayerSnapshot(_points);
        }
    }

    /// <summary>
    /// turn Off/On the start screen
    /// </summary>
    /// <param name="state"></param>
    public void TurnStartMenu(bool state)
    {
        if (startScreen != null)
        {
            startScreen.enabled = state;
        }
    }

    public void StartWheel()
    {
        EventManager.Call_Start();
    }

    public void EndGame()
    {
        FadeUIAway();
    }

    /// <summary>
    /// updae ui to take snapshot on completing a streak
    /// </summary>
    /// <param name="_points"></param>
    private void TakePlayerSnapshot(int _points = 0)
    {
        if (playerSnapshotUI)
        {
            Sprite shot = null;
            if (playerPhotos.Count > 0)
            {
                shot = playerPhotos[Random.Range(0, playerPhotos.Count)];
            }
            playerSnapshotUI.TakeSnapshot(shot, _points);
            EventManager.Call_OnTakeSnapshot(true);

        }
    }


    /// <summary>
    /// update UI based on achieved level and threshold
    /// </summary>
    /// <param name="_level">number of new level</param>
    /// <param name="_points">the exceeded threshold amount</param>
    public void LevelUpUI(int _level, int _points = 0)
    {
        if (badgeImage)
        {
            TryPing(badgeImage.gameObject);
        }
        if (levelTmp)
        {
            levelTmp.text = _level.ToString();
            TryPing(levelTmp.gameObject);
        }
    }

    /// <summary>
    /// update ui on losing enough points
    /// </summary>
    /// <param name="rivalPoints">rival hits a better record</param>
    private void LeftBehindUI()
    {
        Sprite shot = null;
        if (rivalPhotos.Count > 0)
        {
            shot = rivalPhotos[Random.Range(0, rivalPhotos.Count)];
        }
        rivalSnapshotUI.TakeSnapshot(shot, 0);
        EventManager.Call_OnTakeSnapshot(false);
    }

    public bool TryPing(GameObject _obj)
    {
        if (_obj.TryGetComponent(out Pingable levelPing))
        {
            levelPing.Ping();
            return true;
        }
        return false;
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
        EventManager.Call_OnHit(null);
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

    public void SimulatePlayerLevelUp()
    {
        LevelUpUI(3, 2000);
    }

    public void SimulateLeftBehind()
    {
        LeftBehindUI();
    }

    public void SimulateEnd()
    {
        EndGame();
        if (debugPanel)
        {
            debugPanel.gameObject.SetActive(false);
        }
    }

    #endregion


}
