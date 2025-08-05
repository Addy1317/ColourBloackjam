using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SlowpokeStudio.FX
{
    public class FXManager : MonoBehaviour
    {
        [Header("Camera Shake Settings")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _defaultShakeDuration = 0.5f;
        [SerializeField] private float _defaultShakeStrength = 1f;
        [SerializeField] private int _defaultShakeVibrato = 10;
        [SerializeField] private float _defaultShakeRandomness = 90f;

        [Header("UI Buttons Effects")]
        [SerializeField] private float _scaleUp = 1.2f;
        [SerializeField] private float _scaleUpDuration = 0.2f;

        #region Camera Shake Method
        internal void ShakeCameraEffect()
        {
            if (_mainCamera == null)
            {
                Debug.LogError("Main Camera is not assigned in VFXService.");
                return;
            }
            _mainCamera.transform.DOKill();

            _mainCamera.transform.DOShakePosition(
                _defaultShakeDuration,
                _defaultShakeStrength,
                _defaultShakeVibrato,
                _defaultShakeRandomness
            );
        }
        #endregion

        #region UI Buttons Methods
        internal void AddHoverEffect(Button button, float scaleUp = 1.2f, float duration = 0.2f)
        {
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

            EventTrigger.Entry pointerEnter = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerEnter
            };
            pointerEnter.callback.AddListener((eventData) => OnHoverEnter(button, scaleUp, duration));

            EventTrigger.Entry pointerExit = new EventTrigger.Entry
            {
                eventID = EventTriggerType.PointerExit
            };
            pointerExit.callback.AddListener((eventData) => OnHoverExit(button, duration));

            trigger.triggers.Add(pointerEnter);
            trigger.triggers.Add(pointerExit);
        }

        internal void OnHoverEnter(Button button, float scaleUp, float duration)
        {
            button.transform.DOKill();
            button.transform.DOScale(scaleUp, duration).SetEase(Ease.OutBack).SetUpdate(true);
        }

        internal void OnHoverExit(Button button, float duration)
        {
            button.transform.DOKill();
            button.transform.DOScale(1f, duration).SetEase(Ease.InBack).SetUpdate(true);
        }
        #endregion

        #region UI Scale Methods for Number Updates
        internal void ScaleTextUpAndDownEffect(TextMeshProUGUI text, float scaleUp = 1.2f, float scaleDuration = 0.2f, float scaleDown = 1f, float scaleDownDuration = 0.2f)
        {
            text.transform.DOKill();
            text.transform.DOScale(scaleUp, scaleDuration).SetEase(Ease.OutBack).OnKill(() =>
            {
                text.transform.DOScale(scaleDown, scaleDownDuration).SetEase(Ease.InBack);
            });
        }
        #endregion
    }
}

