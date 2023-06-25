using UnityEngine;
using PathCreation;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PathCreator movementPath;
    [SerializeField] private float interpolation;
    [SerializeField] private Transform player;
    private float positionDelta;


    private void Start()
    {
        if (movementPath == null)
        {
            Debug.Log("It's null");
        }
        positionDelta = movementPath.path.length / 2f;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
        {
            Move(true);   
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(false);   
        }
    }


    private void Move(bool rightDirection)
    {
        if (rightDirection)
        {
            positionDelta += 1 * interpolation * Time.deltaTime;
        }
        else
        {
            positionDelta -= 1 * interpolation * Time.deltaTime;
        }
        positionDelta = Mathf.Clamp(positionDelta, 1, movementPath.path.length - 1);

        Vector3 pos = movementPath.path.GetPointAtDistance(positionDelta);
        pos.y = player.position.y;
        player.position = pos;
    }
}
