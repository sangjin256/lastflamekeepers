using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : BehaviourSingleton<GameManager>
{
    public UI_UnitDrawSystem UnitDrawSystem;

    public GameObject DefeatScreen;
    public GameObject WinScreen;

    private void Start()
    {
        Time.timeScale = 1f;
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

        StartCoroutine(WaveManager.Instance.WaveSystemCoroutine());
    }

    public bool IsCreatingUnit()
    {
        if (UnitDrawSystem.gameObject.activeSelf) return true;
        return false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1);
    }

    public void GoToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Success()
    {
        Time.timeScale = 0f;
        WinScreen.SetActive(true);
        AudioManager.Instance.PlayBGM(2);
    }

    public void Defeat()
    {
        Time.timeScale = 0f;
        DefeatScreen.SetActive(true);
        AudioManager.Instance.PlayBGM(3);
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
