using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public void MenuButton()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

    public void ExitButton()
    {
        Application.Quit(0);
    }
}
