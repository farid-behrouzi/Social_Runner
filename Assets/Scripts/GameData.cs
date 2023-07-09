using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameData : MonoBehaviour
{

    private int playerPoint;
    private Dictionary<int, int> basePoints;
    [SerializeField] private List<RewardTableInfo> rewardTableList;

    [SerializeField] private float pointReductionRate;
    [SerializeField] private int pointReductionValue;

    private PlayerBadgeState playerBadgeState;

    private bool rivalSnapshotPermission = false;

    private void Awake()
    {
        playerBadgeState = PlayerBadgeState.None;
        basePoints = new Dictionary<int, int>();
        basePoints.Add(3, 20);
        basePoints.Add(4, 50);
        basePoints.Add(5, 100);
        
        rewardTableList = rewardTableList.ToList().OrderBy(x => x.triggerValue).ToList();
     
    }

    private void Start()
    {
        PlayerToken.OnPlayerHitTokenPointUpdate += UpdatePoint;
        
        InvokeRepeating(nameof(AttentionPointReduction), 0f, pointReductionRate);
    }

    private void UpdatePoint(int tokenCount)
    {
        Debug.Log("Token Count: " + tokenCount);
        if (basePoints.ContainsKey(tokenCount))
        {
            playerPoint += basePoints[tokenCount];
            CheckRewardState();
            GameEvents.PlayerScoreUIUpdate(playerPoint, basePoints[tokenCount]);
        }

        if (playerPoint > 50)
        {
            rivalSnapshotPermission = true;
        }
    }


    private void CheckRewardState()
    {
        foreach (RewardTableInfo item in rewardTableList)
        {
            if (item.state != RewardTable.RivalSnapshot)
            {
                Debug.Log("CheckRewardState: " + item.triggerValue);
                if (playerPoint > item.triggerValue && !item.isTriggered)
                {
                    GrantBadge();
                    item.isTriggered = true;
                }   
            }
        }
    }

    private void GrantBadge()
    {
        Debug.Log("GrantBadge");
        switch (playerBadgeState)
        {
            case PlayerBadgeState.Badge2:
                playerBadgeState = PlayerBadgeState.Badge3;
                break;
            case PlayerBadgeState.Badge1:
                playerBadgeState = PlayerBadgeState.Badge2;
                break;
            case PlayerBadgeState.None:
                playerBadgeState = PlayerBadgeState.Badge1;
                break;
        }
    }

    private void AttentionPointReduction()
    {
        playerPoint -= pointReductionValue;
        if (playerPoint < 0)
        {
            playerPoint = 0;
        }

        CheckReductionSituation();
    }

    private void CheckReductionSituation()
    {
        foreach (RewardTableInfo item in rewardTableList)
        {
            if (item.state == RewardTable.RivalSnapshot && rivalSnapshotPermission)
            {
                if (playerPoint < item.triggerValue && !item.isTriggered)
                {
                    ResetAllRivalSnapShotTriggers();
                    item.isTriggered = true;
                }  
                continue;
            }

            switch (item.state)
            {
                case RewardTable.Badge3:
                    if (playerBadgeState == PlayerBadgeState.Badge3 && playerPoint < item.triggerValue)
                    {
                        playerBadgeState = PlayerBadgeState.Badge2;
                    }
                    break;
                case RewardTable.Badge2:
                    if (playerBadgeState == PlayerBadgeState.Badge2 && playerPoint < item.triggerValue)
                    {
                        playerBadgeState = PlayerBadgeState.Badge1;
                    }
                    break;
                case RewardTable.Badge1:
                    if (playerBadgeState == PlayerBadgeState.Badge1 && playerPoint < item.triggerValue)
                    {
                        playerBadgeState = PlayerBadgeState.None;
                    }
                    break;
            }
        }
    }

    private void ResetAllRivalSnapShotTriggers()
    {
        foreach (RewardTableInfo item in rewardTableList)
        {
            item.isTriggered = false;
        }
    }
    
    
    [System.Serializable]
    public class RewardTableInfo
    {
        public RewardTable state;
        public int triggerValue;
        public bool isTriggered;
    }

}
