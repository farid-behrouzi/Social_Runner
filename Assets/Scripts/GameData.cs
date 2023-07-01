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

    private void Awake()
    {
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
        if (basePoints.ContainsKey(tokenCount))
        {
            playerPoint += basePoints[tokenCount];
        }
    }


    private void CheckRewardState()
    {
        foreach (RewardTableInfo item in rewardTableList)
        {
            if (item.state != RewardTable.RivalSnapshot)
            {
                if (playerPoint > item.triggerValue && !item.isTriggered)
                {
                    GrantBadge(item.state);
                    item.isTriggered = true;
                }   
            }
        }
    }

    private void GrantBadge(RewardTable state)
    {
        
    }

    private void AttentionPointReduction()
    {
        playerPoint -= pointReductionValue;

        CheckRivalSnapShot();
    }

    private void CheckRivalSnapShot()
    {
        foreach (RewardTableInfo item in rewardTableList)
        {
            if (item.state == RewardTable.RivalSnapshot)
            {
                if (playerPoint < item.triggerValue && !item.isTriggered)
                {
                    ResetAllRivalSnapShotTriggers();
                    
                    GrantBadge(item.state);
                    item.isTriggered = true;
                }   
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
