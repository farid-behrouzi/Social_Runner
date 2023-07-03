using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class StreakUI : MonoBehaviour
{
    private List<TokenLightUI> lightsUI = new();
    [SerializeField] private TokenLightUI lightPrefab = null;
    [SerializeField] private RectTransform lightsContainer = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip correctStreakSFX = null;
    [SerializeField] private AudioClip wrongStreakSFX = null;

    private List<Light> nextQueue = null;

    [SerializeField] private bool isAnimPlay = false;

    public void CreateNewStreak(List<Light> _lights)
    {
        nextQueue = _lights;
        if (isAnimPlay)
        {
            return;
        }
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

    public void AnimFinish()
    {
        ClearStreak();
        isAnimPlay = false;
        if (nextQueue != null && nextQueue.Count > 0)
        {
            CreateNewStreak(nextQueue);
            nextQueue.Clear();
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
        if (index < lightsUI.Count)
        {
            if (lightsUI[index] != null && !lightsUI[index].IsOn)
            {
                lightsUI[index].TurnOn(true);
            }
        }
        else
        {
            Debug.LogError("number of light UI elements (" + lightsUI.Count.ToString() + ") is less than index: " + index.ToString());
        }
        
    }

    public void Reset(bool isWon)
    {
        //if (animator && isWon)
        //{
        //    animator.SetTrigger("Right");
        //}
        if (animator)
        {
            animator.SetTrigger(isWon ? "Right" : "Wrong");
        }
        PlayStreakSFX(isWon);
    }



    private void PlayStreakSFX(bool won)
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            /// TODO: fetch Global volume settings
            audioSource.volume = 1;
            if(won && correctStreakSFX)
            {
                audioSource.clip = correctStreakSFX;
            }
            else if (!won && wrongStreakSFX)
            {
                audioSource.clip = wrongStreakSFX;
            }
            audioSource.Play();
        }
    }

    public void SetAnimPlay()
    {
        isAnimPlay = true;
    }
}
