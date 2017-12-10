using UniRx;
using UnitScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UiScripts
{
    [RequireComponent(typeof(UnitHealth))]
    public class HealthUi : MonoBehaviour
    {
        #region Prepared

        [SerializeField] private Slider _slider;                            // The slider to represent how much health the tank currently has.
        [SerializeField] private Image _fillImage;                          // The image component of the slider.
        [SerializeField] private Color _fullHealthColor = Color.green;      // The color the health bar will be when on full health.
        [SerializeField] private Color _zeroHealthColor = Color.red;        // The color the health bar will be when on no health.

        #endregion

        private void Start()
        {
        }

        private void SetHealth(float value)
        {
        }
    }
}