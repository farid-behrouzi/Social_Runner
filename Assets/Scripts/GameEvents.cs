using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public Action<int> OnPlayerHitToken;

    private void Awake()
    {
        if (instance)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    
    
}
