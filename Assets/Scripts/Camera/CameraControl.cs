using UnityEngine;

namespace Camera
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField] private float _velocityDampTime = 0.3f;    // Approximate time for the camera to refocus position.
        [SerializeField] private float _zoomDampTime = 1f;          // Approximate time for the camera to refocus zoom.
        [SerializeField] private Transform _cameraTarget;           // The target the camera needs to encompass.
        [SerializeField] private float _minSize = 10;               // Minimum size of the camera
        [SerializeField] private float _maxSize = 15;               // Maximum size of the camera
        [SerializeField] private float _maximalZoomDistance = 3;    // Distance from the camera to the target on which zoom will be of maximum value

        private UnityEngine.Camera _camera;                                     // Used for referencing the camera.
        private Vector3 _moveVelocity;                              // Reference velocity for the smooth damping of the position.
        private float _zoomSpeed;                                   // Reference speed for the smooth damping of the zoom.

        private void Awake()
        {
            _camera = GetComponentInChildren<UnityEngine.Camera>();
        }


        private void FixedUpdate()
        {
            // Smoothly transition to that target position.
            var desiredPosition = _cameraTarget.position;
            transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _moveVelocity, _velocityDampTime);
            Zoom(desiredPosition - transform.position);
        }

        private void Zoom(Vector3 moveDelta)
        {
            // Find the required size based on the desired position and smoothly transition to that size.
            var zoomFactor = Mathf.Clamp01(moveDelta.magnitude/ _maximalZoomDistance);
            var requiredSize = Mathf.Lerp(_minSize, _maxSize, zoomFactor);
            _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, _zoomDampTime);
        }
    }
}