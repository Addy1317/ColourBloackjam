using TMPro;
using UnityEngine;
using System;

namespace SlowpokeStudio.Timer
{
    public class TimerManager : MonoBehaviour
    {
        [Header("UI Reference")]
        [SerializeField] private TextMeshProUGUI timerText;

        private float remainingTime;
        private bool timerRunning = false;

        public event Action OnTimerEnd; // Optional callback

        public void StartTimer(float timeLimitSeconds)
        {
            remainingTime = timeLimitSeconds;
            timerRunning = true;

            Debug.Log($"[TimerManager] Timer started for {timeLimitSeconds} seconds.");
        }

        private void Update()
        {
            if (!timerRunning) return;

            remainingTime -= Time.deltaTime;

            if (remainingTime <= 0f)
            {
                remainingTime = 0f;
                timerRunning = false;
                Debug.Log("[TimerManager] Timer ended.");
                OnTimerEnd?.Invoke(); // Notify if needed
                GameService.Instance.uiManager.ShowLevelFailedPanel(GameService.Instance.gameManager.currentLevelIndex + 1);
            }

            UpdateTimerDisplay();
        }

        private void UpdateTimerDisplay()
        {
            int minutes = Mathf.FloorToInt(remainingTime / 60f);
            int seconds = Mathf.FloorToInt(remainingTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        public void StopTimer()
        {
            timerRunning = false;
            Debug.Log("[TimerManager] Timer stopped manually.");
        }
    }
}
