using System;
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

    private void OnEnable()
    {
        GameEvents.OnWheelStopped += () => GetComponent<Collider>().enabled = false;
        GameEvents.OnPlayerHitSideCollider += () => gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        GameEvents.OnWheelStopped -= () => GetComponent<Collider>().enabled = false;
        GameEvents.OnPlayerHitSideCollider -= () => gameObject.SetActive(false);
    }

    public override string ToString()
    {
        return "This is a token with ID: " + GetID();
    }

    public Color GetColorFromMaterial()
    {
        return GetComponent<MeshRenderer>().sharedMaterial.color;
    }

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
