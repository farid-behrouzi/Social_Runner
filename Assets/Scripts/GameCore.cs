using UnityEngine;

public class GameCore : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnStart += ActiveGame;
    }

    private void OnDisable()
    {
        EventManager.OnStart -= ActiveGame;
    }


    private void ActiveGame()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
