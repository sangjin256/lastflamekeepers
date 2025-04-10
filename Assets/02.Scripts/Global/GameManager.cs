using UnityEngine;

public class GameManager : BehaviourSingleton<GameManager>
{
    private void Start()
    {
        
    }

    public void Success()
    {
        Debug.Log("성공");
    }

    public void Defeat()
    {
        Debug.Log("패배");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void PlayGame()
    {
        Time.timeScale = 1.0f;
    }

    public void FastGame()
    {
        Time.timeScale = 2.0f;
    }
}
