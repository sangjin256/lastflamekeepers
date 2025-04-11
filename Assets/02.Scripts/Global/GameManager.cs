using UnityEngine;
using System.Collections;

public class GameManager : BehaviourSingleton<GameManager>
{
    public UI_UnitDrawSystem UnitDrawSystem;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    public IEnumerator StartGame()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0f;

        for(int i = 0; i < 3; i++)
        {
            UnitDrawSystem.gameObject.SetActive(true);
            while (UnitDrawSystem.gameObject.activeSelf == true)
            {

                yield return null;
            }
        }

        Time.timeScale = 1f;
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
