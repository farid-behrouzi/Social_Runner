using System;
using System.Collections;
using UnityEngine;

public class WheelMovement : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;

    private float startValue;

    private float endValue;


    private void Awake()
    {
        startValue = rotationSpeed;
        endValue = 0;
    }

    private void Start()
    {
        GameEvents.OnStopWheelSmoothly += StopWheelSmoothly;
        GameEvents.OnCancelWheelReduction += CancelSpeedReduction;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }

    private void CancelSpeedReduction()
    {
        StopCoroutine(nameof(StopWheelSmoothlyCoroutine));
        rotationSpeed = startValue;
    }

    private void StopWheelSmoothly()
    {
        StartCoroutine(nameof(StopWheelSmoothlyCoroutine));
    }

    private IEnumerator StopWheelSmoothlyCoroutine()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < 10f)
        {
            rotationSpeed = Mathf.Lerp(startValue, endValue, elapsedTime / 10f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        GameEvents.OnCancelWheelReduction -= CancelSpeedReduction;
        rotationSpeed = endValue;
        StopCoroutine(nameof(StopWheelSmoothlyCoroutine));
    }
}
