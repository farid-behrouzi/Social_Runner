using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class TokenSpawn : MonoBehaviour
{
    [SerializeField] private Transform spawnPointsParent;
    private Transform[] spawnPointsArray;

    [SerializeField] private Transform tokenPrefab;
    [SerializeField] private Transform tokenParent;

    [SerializeField] private List<Token> tokenPrefabList;


    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        //InvokeRepeating(nameof(SpawnToken), 1, 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SpawnTokenTest(0);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            SpawnTokenTest(1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SpawnTokenTest(2);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SpawnTokenTest(3);
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SpawnTokenTest(4);
        }
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
        tokenPosition = spawnPointsArray[randomPosIndex].position;

        Transform token = TokenPool.SpawnToken(tokenPrefab.gameObject, tokenPosition).transform;
        token.parent = tokenParent;

    }
    
    private void SpawnTokenTest(int index)
    {
        Vector3 tokenPosition = Vector3.zero;
        int randomPosIndex = 2;
        tokenPosition = spawnPointsArray[randomPosIndex].position;

        Transform token = TokenPool.SpawnToken(tokenPrefabList[index].gameObject, tokenPosition).transform;
        token.parent = tokenParent;

    }
}
