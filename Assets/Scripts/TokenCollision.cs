using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenCollision : MonoBehaviour
{
    private const string PLAYER_TAG = "Player";
    private const string DIACTIVATOR_TAG = "Diactivator";
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals(PLAYER_TAG) || other.tag.Equals(DIACTIVATOR_TAG))
        {
            TokenPool.ReturnTokenToPool(gameObject);
        }
    }
}
