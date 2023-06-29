using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] private int ID;
    private TrendStreakType type;
    
    
    public int GetID()
    {
        return ID;
    }

    public void SetTokenType(TrendStreakType type)
    {
        this.type = type;
    }

    public TrendStreakType GetTokenType()
    {
        return type;
    }
}
