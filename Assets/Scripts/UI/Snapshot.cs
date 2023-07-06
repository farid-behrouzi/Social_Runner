using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnityEngine.UI.Image))]
public class Snapshot : MonoBehaviour
{
    [SerializeField] private Animator animator = null;
    [SerializeField] private Image image = null;
    [SerializeField] private string animTriggerLabel = "Take";
    [SerializeField] private TextMeshProUGUI pointsTmp = null;

    private void Awake()
    {
        if (!this.TryGetComponent(out animator))
        {
            Debug.LogError("no animator found in snapshot");
        }
        image.enabled = false;
    }

    public void TakeSnapshot(Sprite _sprite, int points = 0)
    {
        if (pointsTmp)
        {
            pointsTmp.text = points.ToString();
        }

        if (image && _sprite)
        {
            image.sprite = _sprite;
        }

        if (animator)
        {
            animator.SetTrigger(animTriggerLabel);
        }
        
    }
}
