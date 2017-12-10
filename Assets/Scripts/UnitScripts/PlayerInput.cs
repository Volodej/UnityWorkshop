using UnityEngine;

namespace UnitScripts
{
    public class PlayerInput : MonoBehaviour
    {
        #region Prepared

        private TankMover _mover;
        private Aimer _aimer;
        private ShootingCharger _shootingCharger;

        private void Awake()
        {
            _mover = GetComponent<TankMover>();
            _aimer = GetComponent<Aimer>();
            _shootingCharger = GetComponent<ShootingCharger>();
        }
        
        private void Update()
        {
            ApplyMovement();
            ApplyTurn();
            ApplyAim();
            ApplyShooting();
        }

        #endregion

        private void ApplyMovement()
        {
            // Get user input to apply tank movement
        }

        private void ApplyTurn()
        {
            // Get user input to apply tank rotation
        }

        private void ApplyAim()
        {
            // Get user input to apply tank aim
        }

        private void ApplyShooting()
        {
            // Get user input to fire shell from tank
        }
    }
}