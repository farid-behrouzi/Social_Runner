using System.Collections.Generic;
using UnityEngine;

public class PlayerToken : MonoBehaviour
{

    private TrendStreakType playerTrendType;
    private List<Token> playerTokenList = new List<Token>();
    private List<Token> playerPersuadingTrendList = new List<Token>();
    private TokenTrend tokenTrend;


    private void Awake()
    {
        tokenTrend = GameObject.FindObjectOfType<TokenTrend>();
    }

    public void GrabTheToken(TrendStreakType type, int ID, Token token)
    {
        if (playerTokenList.Count == 0)
        {
            SetThePlayerCurrentTrendStreakList(ID);
            return;
        }

        if (IsGrabbedTokenValid(ID))
        {
            playerTokenList.Add(token);
        }
    }



    private void SetThePlayerCurrentTrendStreakList(int ID)
    {
        playerPersuadingTrendList = tokenTrend.WhatIsTheFirstToken(ID);
        if (playerPersuadingTrendList == null)
        {
            return;
        }
        playerTokenList.Add(playerPersuadingTrendList[0]);
    }

    private bool IsGrabbedTokenValid(int ID)
    {
        return playerPersuadingTrendList[playerTokenList.Count].GetID() == ID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Token"))
        {
            Token token = other.gameObject.GetComponent<Token>();
            GrabTheToken(token.GetTokenType(), token.GetID(), token);
        }
    }
    
    
}
