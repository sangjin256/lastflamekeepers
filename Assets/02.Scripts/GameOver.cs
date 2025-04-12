using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene(1); // 메인 게임 씬 이름
    }

    public void OnTitleButton()
    {
        SceneManager.LoadScene(0); // 메인 게임 씬 이름
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
