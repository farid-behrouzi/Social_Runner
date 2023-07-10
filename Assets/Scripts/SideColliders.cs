using UnityEngine;

public class SideColliders : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.OnWheelStopped += ActiveColliders;
        GameEvents.OnPlayerHitSideCollider += DisableColliders;
    }

    private void OnDisable()
    {
        GameEvents.OnWheelStopped -= ActiveColliders;
        GameEvents.OnPlayerHitSideCollider -= DisableColliders;
    }


    private void ActiveColliders()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private void DisableColliders()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
