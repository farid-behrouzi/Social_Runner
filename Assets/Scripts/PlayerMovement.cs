using System;
using System.Collections;
using UnityEngine;
using PathCreation;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private PathCreator movementPath;
    [SerializeField] private float interpolation;
    [SerializeField] private Transform player;
    private float positionDelta;
    
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float jumpDuration = 1f;
    private bool isJumping = false;
    private float jumpTimer = 0f;

    [SerializeField] private AnimationCurve hightCurve;

    private bool limitMovement = false;


    private void Start()
    {
        positionDelta = movementPath.path.length / 2f;
        GameEvents.OnWheelStopped += () => limitMovement = true;
        GameEvents.OnPlayerHitSideCollider += () => enabled = false;
    }

    private void OnDisable()
    {
        GameEvents.OnWheelStopped -= () => limitMovement = true;
        GameEvents.OnPlayerHitSideCollider -= () => enabled = false;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            Move(true);   
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            Move(false);   
        }

        if (limitMovement)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            isJumping = true;
            jumpTimer = 0f;
            StartCoroutine(Jump());
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            GameEvents.Call_OnStopWheelSmoothly();
        }
        
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            GameEvents.Call_OnCancelWheelSpeedReduction();
        }

        // if (Input.GetKeyDown(KeyCode.H))
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // }
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

    private IEnumerator Jump()
    {
        Vector3 playerPos = Vector3.up;
        while (isJumping)
        {
            jumpTimer += Time.deltaTime;
            float height = hightCurve.Evaluate(jumpTimer);
            playerPos = new Vector3(player.localPosition.x, height, player.localPosition.z);
            player.localPosition = playerPos;

            if (jumpTimer >= jumpDuration)
            {
                isJumping = false;
                yield break;
            }
            
            yield return null;
        }
    }
}
