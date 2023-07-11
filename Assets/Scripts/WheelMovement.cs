using System.Collections;
using UnityEngine;

public class WheelMovement : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;

    private float startValue;

    private float endValue;

    private float rangedSpeed = 1f;

    [SerializeField] private float stopTime;
    private float defaultRotationSpeed;


    private void Awake()
    {
        defaultRotationSpeed = rotationSpeed;
        startValue = rangedSpeed;
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
        transform.Rotate (Vector3.right * rotationSpeed * rangedSpeed * Time.deltaTime, Space.Self);
    }

    private void CancelSpeedReduction()
    {
        StopCoroutine(nameof(StopWheelSmoothlyCoroutine));
        rotationSpeed = defaultRotationSpeed;
        rangedSpeed = 1;
    }

    private void StopWheelSmoothly()
    {
        StartCoroutine(nameof(StopWheelSmoothlyCoroutine));
    }

    private IEnumerator StopWheelSmoothlyCoroutine()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < stopTime)
        {
            rangedSpeed = Mathf.Lerp(startValue, endValue, elapsedTime / 10f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        GameEvents.OnCancelWheelReduction -= CancelSpeedReduction;
        rangedSpeed = endValue;
        Debug.Log("StopWheelSmoothlyCoroutine");
        GameEvents.Call_OnWheelStopped();
        StopCoroutine(nameof(StopWheelSmoothlyCoroutine));
    }
}
