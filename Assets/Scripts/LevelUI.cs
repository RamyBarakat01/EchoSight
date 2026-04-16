using System.Collections;
using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [Header("Objective UI")]
    public TMP_Text objectiveText;
    public float objectiveDisplayTime = 3f;

    [Header("Kill Counter UI")]
    public TMP_Text killCounterText;

    void Start()
    {
        if (objectiveText != null)
        {
            objectiveText.text = "Objective: Kill all the enemies.";
            StartCoroutine(HideObjectiveAfterDelay());
        }

        UpdateKillCounter();
    }

    void Update()
    {
        UpdateKillCounter();
    }

    IEnumerator HideObjectiveAfterDelay()
    {
        yield return new WaitForSeconds(objectiveDisplayTime);

        if (objectiveText != null)
        {
            objectiveText.gameObject.SetActive(false);
        }
    }

    void UpdateKillCounter()
    {
        if (killCounterText != null && GameManager.Instance != null)
        {
            int killed = GameManager.Instance.GetEnemiesKilled();
            int total = GameManager.Instance.GetTotalEnemies();

            killCounterText.text = killed + "/" + total;
        }
    }
}