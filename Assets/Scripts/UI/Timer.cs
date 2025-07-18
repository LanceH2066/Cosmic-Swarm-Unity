using UnityEngine;
using TMPro;

public class RunTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    private float elapsedTime = 0f;
    private float maxTime = 1800f;
    private bool timerRunning = true;

    void Update()
    {
        if (timerRunning && elapsedTime < maxTime)
        {
            elapsedTime += Time.deltaTime;
            elapsedTime = Mathf.Min(elapsedTime, maxTime);
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public void ResumeTimer()
    {
        timerRunning = true;
    }
}
