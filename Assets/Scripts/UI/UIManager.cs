using SlowpokeStudio.Audio;
using SlowpokeStudio.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace SlowpokeStudio
{
    public class UIManager : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;

        [Header("Game Settings")]
        [SerializeField] private Button _settingsButton;
        [SerializeField] private GameObject _gameSettingsPanel;
        [SerializeField] private Button _homeButton;
        [SerializeField] private Button _quitButton;
        [SerializeField] private Button _settingsCloseButton;

        [Header("Game UI")]
        [SerializeField] private TextMeshProUGUI _currentLevelText;
        [SerializeField] private TextMeshProUGUI _currentGoldText;
        [SerializeField] private TextMeshProUGUI _timerText;
        [SerializeField] private Button _levelRestartButton;

        [Header("Level Complete Panel")]
        [SerializeField] private GameObject _levelCompletePanel;
        [SerializeField] private TextMeshProUGUI _completedLevelText;
        [SerializeField] private TextMeshProUGUI _nextLevelText;
        [SerializeField] private Button _nextButton;

        [Header("Level Failed Panel")]
        [SerializeField] private GameObject _levelFailedPanel;
        [SerializeField] private TextMeshProUGUI _failedLevelText;
        [SerializeField] private Button _retryButton;

        private void Start()
        {
            _mainMenuPanel.SetActive(true);
            _gameSettingsPanel.SetActive(false);
            _levelCompletePanel.SetActive(false);
            _currentLevelText.transform.parent.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(OnStartGameClicked);
            _exitGameButton.onClick.AddListener(OnExitGameClicked);
            _settingsCloseButton.onClick.AddListener(OnCloseSettingsClicked);
            _nextButton.onClick.AddListener(OnNextLevelClicked);
            _settingsButton.onClick.AddListener(OnToggleSettingsPanel);
            _levelRestartButton.onClick.AddListener(OnRestartLevelClicked);
            _retryButton.onClick.AddListener(OnRetryClicked);

            _homeButton.onClick.AddListener(OnHomeButtonClicked);
            _quitButton.onClick.AddListener(OnExitGameClicked);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(OnStartGameClicked);
            _exitGameButton.onClick.RemoveListener(OnExitGameClicked);
            _settingsCloseButton.onClick.RemoveListener(OnCloseSettingsClicked);
            _nextButton.onClick.RemoveListener(OnNextLevelClicked);
            _settingsButton.onClick.RemoveListener(OnToggleSettingsPanel);
            _levelRestartButton.onClick.RemoveListener(OnRestartLevelClicked);
            _retryButton.onClick.RemoveListener(OnRetryClicked);

            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
            _quitButton.onClick.RemoveListener(OnExitGameClicked);
        }

        public void ShowLevelCompletePanel(int currentLevel, int nextLevel)
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnLevelCompleteSFX);
            _levelCompletePanel.SetActive(true);
            _completedLevelText.text = $"Level {currentLevel} Completed!";
            _nextLevelText.text = $"Next: Level {nextLevel}";

            Debug.Log($"[UIManager] Showing Level Complete Panel for Level {currentLevel}");
        }

        private void OnNextLevelClicked()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);

            Debug.Log("[UIManager] Next Level button clicked.");
            _levelCompletePanel.SetActive(false);

            int nextLevel = GameService.Instance.gameManager.currentLevelIndex + 1;

            SaveManager.SaveCurrentLevel(nextLevel);  // ✅ Save the next level for restart

            GameService.Instance.gameManager.LoadLevel(nextLevel);
        }

        public void UpdateLevelText(int levelNumber)
        {
            _currentLevelText.text = $"{levelNumber}";
            Debug.Log($"[UIManager] Updated Level Text to: Level {levelNumber}");
        }

        private void OnRestartLevelClicked()
        {
            Debug.Log("[UIManager] Restart button clicked. Reloading current level...");
            GameService.Instance.gameManager.LoadLevel(GameService.Instance.gameManager.currentLevelIndex);
        }

        public void ShowLevelFailedPanel(int failedLevel)
        {
            _levelFailedPanel.SetActive(true);
            _failedLevelText.text = $"Level {failedLevel} Failed!";
            Debug.Log($"[UIManager] Showing Level Failed Panel for Level {failedLevel}");
        }

        private void OnRetryClicked()
        {
            Debug.Log("[UIManager] Retry button clicked. Restarting level...");
            _levelFailedPanel.SetActive(false);
            GameService.Instance.gameManager.LoadLevel(GameService.Instance.gameManager.currentLevelIndex);
        }

        #region Buttons Methods

        private void OnStartGameClicked()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            Debug.Log("[UIManager] Play button clicked. Starting game...");
            _mainMenuPanel.SetActive(false);
            _gameSettingsPanel.SetActive(false);
            _currentLevelText.transform.parent.gameObject.SetActive(true); // Show gameplay UI group

            //GameService.Instance.gameManager.LoadLevel(0); // Start first level
            int savedLevel = SaveManager.GetCurrentLevel();
            int levelToLoad = (savedLevel <= 0) ? 0 : savedLevel;

            Debug.Log($"[UIManager] Loading Level: {levelToLoad}");

            GameService.Instance.gameManager.LoadLevel(levelToLoad);
        }

        private void OnExitGameClicked()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            Debug.Log("[UIManager] Exit button clicked.");
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

        private void OnCloseSettingsClicked()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);

            Debug.Log("[UIManager] Settings closed.");
            _gameSettingsPanel.SetActive(false);
        }

        // Optional: Public toggle for Settings Panel
        public void OnToggleSettingsPanel()
        {
            GameService.Instance.audioManager.PlaySFX(SFXType.OnButtonClickSFX);
            _gameSettingsPanel.SetActive(!_gameSettingsPanel.activeSelf);
        }

        private void OnHomeButtonClicked()
        {
            Debug.Log("[UIManager] Home button clicked. Returning to Main Menu...");

            _mainMenuPanel.SetActive(true);
            _gameSettingsPanel.SetActive(false);
            _levelCompletePanel.SetActive(false);
            _levelFailedPanel.SetActive(false);
            _currentLevelText.transform.parent.gameObject.SetActive(false); // Hide gameplay UI

            // Optional: Stop timer and clear current game state
            GameService.Instance.timerManager.StopTimer();
        }


        #endregion
    }
}

