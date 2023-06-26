using UnityEngine;
using Random = UnityEngine.Random;

public class TokenSpawn : MonoBehaviour
{
    [SerializeField] private Transform leftLimitSpawnPosition;
    [SerializeField] private Transform rightLimitSpawnPosition;

    [SerializeField] private Transform tokenPrefab;
    [SerializeField] private Transform tokenParent;


    private void Start()
    {
        InvokeRepeating(nameof(SpawnToken), 1, 1);
    }

    private void SpawnToken()
    {
        Vector3 tokenPosition = Vector3.zero;
        tokenPosition.y = leftLimitSpawnPosition.position.y;
        tokenPosition.z = leftLimitSpawnPosition.position.z;
        tokenPosition.x = Random.Range(leftLimitSpawnPosition.position.x, rightLimitSpawnPosition.position.x);

        Transform token = TokenPool.SpawnToken(tokenPrefab.gameObject, tokenPosition).transform;
        token.parent = tokenParent;

    }
}
