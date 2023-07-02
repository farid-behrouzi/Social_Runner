using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.UI;

public class TokenLightUI : MonoBehaviour
{
    [SerializeField] private Image token = null;
    [SerializeField] private Image frame = null;
    [SerializeField] private Animator animator = null;
    [SerializeField] private string animBoolState = "On";

    private bool isOn = false;

    private void Awake()
    {
        if (animator == null)
        {
            animator = this.gameObject.GetComponent<Animator>();
        }
    }

    public void TurnOn(bool state)
    {
        if (animator)
        {
            animator.SetBool(animBoolState, state);
        }
        else if (frame)
        {
            frame.color = new Color(frame.color.r, frame.color.g, frame.color.b, state ? 1 : 0);
            if (this.gameObject.TryGetComponent(out Pingable pinger))
            {
                pinger.Ping();
            }
        }
        isOn = state;
    }

    public void SetLight(Light _light)
    {
        if (token && frame)
        {
            if (_light.sprite != null)
            {
                token.sprite = _light.sprite;
            }
            token.color = _light.color;
            if (_light.active && !isOn)
            {
                TurnOn(true);
            }
            else if (!_light.active)
            {
                TurnOn(false);
            }
        }
    }

    public bool IsOn
    {
        get { return isOn; }
    }
}
