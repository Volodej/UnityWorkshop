using UnityEngine;

namespace UiScripts
{
    /// <summary>
    /// This class is used to make sure world space UI
    /// elements such as the health bar face the correct direction. 
    /// </summary>
    public class UiDirectionControl : MonoBehaviour
    {
        [SerializeField] private Quaternion _relativeRotation;          // The local rotation at the start of the scene.

        private void Start()
        {
            _relativeRotation = transform.parent.localRotation;
        }
        
        private void Update()
        {
                transform.rotation = _relativeRotation;
        }
    }
}