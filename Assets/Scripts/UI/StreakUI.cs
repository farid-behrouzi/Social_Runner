using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreakUI : MonoBehaviour
{
    private List<TokenLightUI> lightsUI = new();
    [SerializeField] private TokenLightUI lightPrefab = null;
    [SerializeField] private RectTransform lightsContainer = null;

    public void CreateNewStreak(List<Light> _lights)
    {
        if (lightsContainer == null)
        {
            Debug.LogError("no lights container found");
            return;
        }

        if (lightPrefab == null)
        {
            Debug.LogError("no lightPrefab found");
            return;
        }

        ClearStreak();

        foreach(Light l in _lights)
        {
            TokenLightUI tokenLight = Instantiate(lightPrefab, lightsContainer) as TokenLightUI;
            lightsUI.Add(tokenLight);
            tokenLight.SetLight(l);
        }

    }


    public void ClearStreak()
    {
        if (lightsContainer != null)
        {
            lightsUI.Clear();
            for (int i = 0; i < lightsContainer.childCount; i++)
            {
                Destroy(lightsContainer.GetChild(i).gameObject);
            }
        }
    }

    public void TurnLight(int index)
    {
        if (lightsUI[index] != null && !lightsUI[index].IsOn)
        {
            lightsUI[index].TurnOn(true);
        }
    }
}
