using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] private int ID;
    private TrendStreakType type;


    // private void Start()
    // {
    //     Material material = gameObject.GetComponent<MeshRenderer>().sharedMaterial;
    //     color = material.color;
    // }

    public Color GetColor(TokenTrend token)
    {
        return token.GetTokenColor(ID);
    }

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
