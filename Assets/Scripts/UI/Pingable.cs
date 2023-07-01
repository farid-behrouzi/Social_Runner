using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class Pingable : MonoBehaviour
{
    [SerializeField] private string labelID = "";
    [SerializeField] private RectTransform rect = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private float runTime = 0.2f;
    [Range(1, 2)]
    [SerializeField] private float maxScale = 1.1f;
    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip audioClip = null;

    private float timer = 0.0f;
    private Vector3 initialScale = Vector3.one;
    private float stage = 0.1f;

    private void OnEnable()
    {
        EventManager.OnPing += Do_OnPing;
    }

    private void OnDisable()
    {
        EventManager.OnPing -= Do_OnPing;
    }

    private void Do_OnPing(string _labelID)
    {
        if (_labelID == labelID)
        {
            Ping();
        }
    }

    private void Awake()
    {
        if (rect == null && !this.gameObject.TryGetComponent(out rect))
        {
            Debug.LogError("Rect transform is required", this);
        }
        else
        {
            initialScale = rect.localScale;
        }
        if (audioSource == null)
        {
            this.gameObject.TryGetComponent(out audioSource);
        }
        if (audioSource!= null)
        {
            audioSource.playOnAwake = false;
        }
        stage = runTime / 2;
    }

    private void Update()
    {
        if (rect && timer > 0)
        {
            float p;
            if (timer >= stage)
            {
                p = 1 - ((timer - stage) / stage);
            }
            else
            {
                p = timer / stage;
            }
            float m = Mathf.Lerp(1, maxScale, p);
            rect.localScale = initialScale * m;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                rect.localScale = initialScale;
            }
        }
    }

    public void Ping()
    {
        if (animator)
        {
            animator.SetTrigger("Ping");
        }
        else if (rect)
        {
            timer = runTime;
        }
        PlaySFX();
    }

    private void PlaySFX()
    {
        if (audioSource)
        {
            audioSource.Stop();
            /// TODO: get global sfx volume settins
            audioSource.volume = 1;
            audioSource.Play();
        }
    }
}
