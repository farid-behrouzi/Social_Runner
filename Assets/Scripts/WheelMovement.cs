using UnityEngine;

public class WheelMovement : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate (Vector3.right * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
