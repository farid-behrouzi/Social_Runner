using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TokenPool : MonoBehaviour
{

    public static List<PooledObjectInfo> objectPoolsList = new List<PooledObjectInfo>();

    public static GameObject SpawnToken(GameObject tokenToSpawn, Vector3 tokenPosition)
    {
        PooledObjectInfo pooledToken = objectPoolsList.Find(p => p.lookupString == tokenToSpawn.name);

        if (pooledToken == null)
        {
            pooledToken = new PooledObjectInfo() { lookupString = tokenToSpawn.name };
            objectPoolsList.Add(pooledToken);
        }

        GameObject spawnableToken = pooledToken.inactiveObjectsList.FirstOrDefault();

        if (spawnableToken == null)
        {
            spawnableToken = Instantiate(tokenToSpawn, tokenPosition, Quaternion.identity);
        }
        else
        {
            spawnableToken.transform.position = tokenPosition;
            pooledToken.inactiveObjectsList.Remove(spawnableToken);
            spawnableToken.SetActive(true);
        }

        return spawnableToken;
    }

    public static void ReturnTokenToPool(GameObject token)
    {
        string tokenName = token.name.Substring(0, token.name.Length - 7);
        PooledObjectInfo pool = objectPoolsList.Find(p => p.lookupString == tokenName);

        if (pool == null)
        {
            Debug.LogWarning("trying to release an object that is not pooled yet: " + token.name);
        }
        else
        {
            token.SetActive(false);
            pool.inactiveObjectsList.Add(token);
        }
    }

    public class PooledObjectInfo
    {
        public string lookupString;
        public List<GameObject> inactiveObjectsList = new List<GameObject>();
    }
}
