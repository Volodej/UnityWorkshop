using UniRx;
using UnitScripts;
using UnityEngine;
using UnityEngine.UI;

namespace UiScripts
{
    public class AimUi : MonoBehaviour
    {
        [SerializeField] private Slider _aimSlider;                  // A child of the tank that displays the current launch force.
        [SerializeField] private RectTransform _sliderRoot;          // Root of slider that will rotate with the cannon

        private void Start()
        {
            GetComponent<Aimer>().AimRotation.Subscribe(rotation => _sliderRoot.localRotation = rotation);
            GetComponent<ShootingCharger>().ChargingForceStream.Subscribe(force => _aimSlider.value = force);
        }
    }
}