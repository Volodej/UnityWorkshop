using UniRx;
using UnitScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UiScripts
{
    public class AimUi : MonoBehaviour
    {
        [SerializeField] private Slider _aimSlider;                  // A child of the tank that displays the current launch force.
        [SerializeField] private RectTransform _sliderRoot;          // Root of slider that will rotate with the cannon.

        private void Start()
        {
        }

        private void SetRotation(float angle)
        {
            // Set rotation for slider by Z axis.
        }
    }
}