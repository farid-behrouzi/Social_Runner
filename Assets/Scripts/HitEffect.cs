using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem hitEffectParticleSystem = null;
    private void OnEnable()
    {
        EventManager.OnHit += Play;
    }

    private void OnDisable()
    {
        EventManager.OnHit -= Play;
    }

    private void Play(Token _token)
    {
        if (hitEffectParticleSystem != null && _token != null)
        {
            var main = hitEffectParticleSystem.main;
            main.startColor = _token.GetColorFromMaterial();
            hitEffectParticleSystem.Play();
        }
    }
}
