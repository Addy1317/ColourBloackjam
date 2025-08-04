using SlowpokeStudio.ColourBlocks;
using SlowpokeStudio.Generic;
using SlowpokeStudio.Grid;
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
        //[SerializeField] internal AudioManager audioManager;
        //[SerializeField] internal BulletManager bulletManager;
        //[SerializeField] internal CurrencyManager currencyManager;       
        [SerializeField] internal GridManager gridManager;
        [SerializeField] internal UIManager uiManager;
        //[SerializeField] internal VFXManager vfxManager;
        //[SerializeField] internal EventManager eventManager;
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
            //{ "AudioManager", audioManager },
            //{ "BulletManager", bulletManager },
            //{ "CurrencyManager", currencyManager },

           // { "TowerManager", towerManager },
            { "CustomGridManager", gridManager },
            //{ "UIManager", uiManager },
            //{ "VFXManager", vfxManager },
            //{ "EventManager", eventManager }
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
