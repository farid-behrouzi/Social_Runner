using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[Serializable]
public class ScorePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro tmp = null;
    [SerializeField] Animator anim = null;
    [SerializeField] string triggerLabel = "Active";
    [SerializeField] Color positiveAmountColor = Color.green;
    [SerializeField] Color negativeAmountColor = Color.red;
    [SerializeField] AudioSource audioSource = null;
    [SerializeField] AudioClip sfx = null;

    private float lifeTime = 1.0f;
    private int score = 0;

    private void Awake()
    {
        if (tmp == null && !this.gameObject.TryGetComponent(out tmp))
        {
            Debug.LogWarning(" no tmpro found in Score Popup");
        }

        if (anim == null && !this.gameObject.TryGetComponent(out anim))
        {
            Debug.LogWarning(" no Animator found in Score Popup");
        }

        if (audioSource == null && !this.gameObject.TryGetComponent(out audioSource))
        {
            Debug.LogWarning(" no Audio Source found in Score Popup");
        }
        else
        {
            audioSource.playOnAwake = false;
            //audioSource.volume = GLOBAL_SFX_VOLUME_USER)SETTINGS
        }

    }

    public void Init()
    {
        SetText();
        StartCoroutine(Show());
    }

    private void SetText()
    {
        if (tmp != null)
        {
            tmp.text = string.Empty;
            if (score >= 0)
            {
                tmp.text = "+";
                tmp.color = positiveAmountColor;
            }
            else
            {

                tmp.text = "-";
                tmp.color = negativeAmountColor;
            }
            tmp.text += score.ToString();
        }
    }

    IEnumerator Show()
    {
        PlaySFX();
        PlayAnim();
        yield return new WaitForSeconds(LifeTime);
        Hide();
    }

    private void PlayAnim()
    {
        if (anim != null)
        {
            anim.SetTrigger(triggerLabel);
        }
    }

    private void Hide()
    {
        Destroy(this.gameObject);
    }

    private void PlaySFX()
    {
        if (audioSource != null && sfx != null)
        {
            audioSource.Stop();
            audioSource.clip = sfx;
            audioSource.Play();
        }
    }

    public float LifeTime
    {
        get { return lifeTime; }
        set { lifeTime = value;}
    }

    public int Score
    {
        get { return score; }
        set { score = value;}
    }

    public AudioClip SFX
    {
        get { return sfx; }
        set { sfx = value;}
    }
}
