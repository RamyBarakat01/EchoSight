using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject signupPanel;

    [Header("Status UI")]
    public TMPro.TMP_Text statusText;

    void Start()
    {
        if (loginPanel != null)
            loginPanel.SetActive(false);

        if (signupPanel != null)
            signupPanel.SetActive(false);
    }

    public void OpenLoginPanel()
    {
        if (loginPanel != null)
            loginPanel.SetActive(true);

        if (signupPanel != null)
            signupPanel.SetActive(false);

        SetStatus("Enter your login details.");
    }

    public void OpenSignupPanel()
    {
        if (signupPanel != null)
            signupPanel.SetActive(true);

        if (loginPanel != null)
            loginPanel.SetActive(false);

        SetStatus("Create a new account.");
    }

    public void StartGame()
    {
        if (FirebaseManager.Instance != null && FirebaseManager.Instance.IsLoggedIn())
        {
            SceneManager.LoadScene("MainPrototype");
        }
        else
        {
            SetStatus("You must log in first.");
            Debug.Log("Please log in first.");
        }
    }

    public void ExitGame()
    {
        Debug.Log("Quit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}