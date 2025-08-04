using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlowpokeStudio
{
    public class UIManager : MonoBehaviour
    {
        [Header("Level Complete Panel")]
        [SerializeField] private GameObject levelCompletePanel;
        [SerializeField] private TextMeshProUGUI completedLevelText;
        [SerializeField] private TextMeshProUGUI nextLevelText;
        [SerializeField] private Button nextButton;

        private void Awake()
        {
            // Hide panel on start
            levelCompletePanel.SetActive(false);

            // Bind button click
            nextButton.onClick.AddListener(OnNextLevelClicked);
        }

        public void ShowLevelCompletePanel(int currentLevel, int nextLevel)
        {
            levelCompletePanel.SetActive(true);
            completedLevelText.text = $"Level {currentLevel} Completed!";
            nextLevelText.text = $"Next: Level {nextLevel}";

            Debug.Log($"[UIManager] Showing Level Complete Panel for Level {currentLevel}");
        }

        private void OnNextLevelClicked()
        {
            Debug.Log("[UIManager] Next Level button clicked.");
            levelCompletePanel.SetActive(false);

            // Trigger GameManager to load next level
           GameService.Instance.gameManager.LoadLevel(GameService.Instance.gameManager.currentLevelIndex + 1);
        }
    }
}

