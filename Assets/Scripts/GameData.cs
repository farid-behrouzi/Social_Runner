using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int attentionPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    
}
