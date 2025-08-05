using SlowpokeStudio.Audio;
using SlowpokeStudio.ColourBlocks;
using SlowpokeStudio.Currency;
using SlowpokeStudio.Generic;
using SlowpokeStudio.Grid;
using SlowpokeStudio.Timer;
using SlowpokeStudio.Wall;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace SlowpokeStudio
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        [Header("Service")]
        [SerializeField] internal GameManager gameManager;
        [SerializeField] internal AudioManager audioManager;
        [SerializeField] internal CurrencyManager currencyManager;       
        [SerializeField] internal GridManager gridManager;
        [SerializeField] internal UIManager uiManager;
        //[SerializeField] internal VFXManager vfxManager;
        [SerializeField] internal TimerManager timerManager;
        [SerializeField] internal WallManager wallManager;
        [SerializeField] internal BlockManager blockManager;

        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
            {
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeServices();
        }

        private void InitializeServices()
        {
            var services = new Dictionary<string, Object>
            {
            { "GameManager", gameManager },
            { "AudioManager", audioManager },
            { "GridManager", gridManager },
            { "WallManager", wallManager },
            { "UIManager", uiManager },
            { "BlockManager", blockManager },
            { "TimerManager", timerManager },
            { "CurrencyManager", currencyManager }
            };

            foreach (var service in services)
            {
                if (service.Value == null)
                {
                    Debug.LogError($"{service.Key} failed to initialize.");
                }
                else
                {
                    Debug.Log($"{service.Key} is initialized.");
                }
            }
        }
    }
}
