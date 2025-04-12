using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    public void OnStartButton()
    {
        SceneManager.LoadScene(1); // 메인 게임 씬 이름
    }

    public void OnSettingButton(GameObject settingPanel)
    {
        settingPanel.SetActive(true);
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
