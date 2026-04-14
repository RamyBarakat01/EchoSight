using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainPrototype");
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();
    }
}