using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TokenSpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPointsParent;
    private Transform[] spawnPointsArray;
    
    [SerializeField] private Transform tokenParent;

    [SerializeField] private List<Token> tokenPrefabList;

    [SerializeField] private float tokenSpawnMinimumRate;
    [SerializeField] private float tokenSpawnMaximumRate;
    private float spawnTimer;
    private float spawnGap;

    [SerializeField] private float trendTokenChanceToSpawn;

    [SerializeField] private bool spawnByChance;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        GameEvents.OnWheelStopped += () => enabled = false;
    }

    private void Update()
    {
        SpawnTimer();
        // if (Input.GetKeyDown(KeyCode.Z))
        // {
        //     SpawnTokenTest(0);
        // }
        // if (Input.GetKeyDown(KeyCode.X))
        // {
        //     SpawnTokenTest(1);
        // }
        // if (Input.GetKeyDown(KeyCode.C))
        // {
        //     SpawnTokenTest(2);
        // }
        // if (Input.GetKeyDown(KeyCode.V))
        // {
        //     SpawnTokenTest(3);
        // }
        // if (Input.GetKeyDown(KeyCode.B))
        // {
        //     SpawnTokenTest(4);
        // }
    }

    private void Initialize()
    {
        spawnPointsArray = new Transform[spawnPointsParent.childCount];

        for (int i = 0; i < spawnPointsArray.Length; i++)
        {
            spawnPointsArray[i] = spawnPointsParent.GetChild(i);
        }
    }

    private void SpawnToken()
    {
        Vector3 tokenPosition = Vector3.zero;
        int randomPosIndex = Random.Range(0, spawnPointsArray.Length);
        tokenPosition = spawnPointsArray[randomPosIndex].transform.position;

        GameObject tokenPrefab = GetTokenToSpawn();
        Transform token = TokenPool.SpawnToken(tokenPrefab, tokenPosition).transform;
        token.parent = tokenParent;

        SwapWithLastIndex(randomPosIndex);
        
        randomPosIndex = Random.Range(0, spawnPointsArray.Length - 1);
        tokenPosition = spawnPointsArray[randomPosIndex].transform.position;

        GameObject secondTokenPrefab = GetTokenToSpawn();
        Transform secondToken = TokenPool.SpawnToken(secondTokenPrefab, tokenPosition).transform;
        secondToken.parent = tokenParent; 
    }

    private GameObject GetTokenToSpawn()
    {
        if (spawnByChance)
        {
            float chance = Random.value;
            if (chance < trendTokenChanceToSpawn / 100f)
            {
                List<Token> currentTrendTokens = GameEvents.GetCurrentTrendStreakList();
                return currentTrendTokens[Random.Range(0, currentTrendTokens.Count)].gameObject;
            }
            return tokenPrefabList[Random.Range(0, tokenPrefabList.Count)].gameObject;
        }
        Debug.Log("GetTokenToSpawn");
        return tokenPrefabList[Random.Range(0, tokenPrefabList.Count)].gameObject;
    }
    
    private void SpawnTokenTest(int index)
    {
        Vector3 tokenPosition = Vector3.zero;
        int randomPosIndex = 2;
        tokenPosition = spawnPointsArray[randomPosIndex].position;

        Transform token = TokenPool.SpawnToken(tokenPrefabList[index].gameObject, tokenPosition).transform;
        token.parent = tokenParent;

    }

    private void SpawnTimer()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnGap)
        {
            spawnTimer = 0;
            SetNewSpawnGap();
            SpawnToken();
        }
    }

    private void SetNewSpawnGap()
    {
        spawnGap = Random.Range(tokenSpawnMinimumRate, tokenSpawnMaximumRate);
    }
    
    private void SwapWithLastIndex(int index)
    {
        if (index < 0 || index >= spawnPointsArray.Length)
        {
            Debug.LogError("Invalid index!");
            return;
        }

        // Swapping the value at the specified index with the last index
        Transform temp = spawnPointsArray[index];
        spawnPointsArray[index] = spawnPointsArray[spawnPointsArray.Length - 1];
        spawnPointsArray[spawnPointsArray.Length - 1] = temp;
    }
}
