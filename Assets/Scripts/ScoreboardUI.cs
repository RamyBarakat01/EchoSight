using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreboardUI : MonoBehaviour
{
    public GameObject scoreboardPanel;

    public TMP_Text titleText;
    public TMP_Text killScoreText;
    public TMP_Text timeText;
    public TMP_Text timeBonusText;
    public TMP_Text finalScoreText;

    private bool shown = false;

    void Update()
    {
        if (!shown && GameManager.Instance != null && GameManager.Instance.gameEnded)
        {
            ShowScoreboard();
        }
    }

    void ShowScoreboard()
    {
        shown = true;

        scoreboardPanel.SetActive(true);

        int killScore = GameManager.Instance.GetKillScore();
        float elapsedTime = GameManager.Instance.GetElapsedTime();
        int finalScore = GameManager.Instance.GetFinalScore();
        int timeBonus = finalScore - killScore;

        if (GameManager.Instance.levelCompleted)
            titleText.text = "Level Complete";
        else if (GameManager.Instance.playerDied)
            titleText.text = "Game Over";

        killScoreText.text = "Kill Score: " + killScore;
        timeText.text = "Time: " + elapsedTime.ToString("F2") + "s";
        timeBonusText.text = "Time Bonus: " + timeBonus;
        finalScoreText.text = "Final Score: " + finalScore;

        Time.timeScale = 0f;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}