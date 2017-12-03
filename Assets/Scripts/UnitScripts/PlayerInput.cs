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
            if (!Input.anyKey)
                return;

            ApplyMovement();
            ApplyTurn();
            ApplyAim();
            ApplyShooting();
        }

        #endregion

        private void ApplyMovement()
        {
            // Get user input to apply tank movement
            var moveForward = Input.GetKey(KeyCode.W) ? 1 : 0;
            var moveBack = Input.GetKey(KeyCode.S) ? 1 : 0;
            var speedFactor = moveForward - moveBack;
            _mover.Move(speedFactor);
        }

        private void ApplyTurn()
        {
            // Get user input to apply tank rotation
            var turnLeft = Input.GetKey(KeyCode.A) ? 1 : 0;
            var turnRight = Input.GetKey(KeyCode.D) ? 1 : 0;
            var turnFactor = turnRight - turnLeft;
            _mover.Turn(turnFactor);
        }

        private void ApplyAim()
        {
            // Get user input to apply tank aim
            var aimLeft = Input.GetKey(KeyCode.LeftArrow) ? 1 : 0;
            var aimRight = Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
            var aimFactor = aimRight - aimLeft;
            _aimer.Aim(aimFactor);
        }

        private void ApplyShooting()
        {
            // Get user input to fire shell from tank
            if (Input.GetKeyDown(KeyCode.Space))
                _shootingCharger.StartShotCharging();

            if (Input.GetKeyDown(KeyCode.Space))
                _shootingCharger.EndShotCharging();
        }
    }
}