using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public void PlayButton()
    {
        Debug.Log("Play button presssed");
        SceneManager.LoadSceneAsync("Game");
    }

    public void ExitButton()
    {
        Debug.Log("Exit button pressed");
        Application.Quit(0);
    }
}
