using UnityEngine;

namespace UnitScripts
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private Rigidbody _shell;                   // Prefab of the shell.
        [SerializeField] private Transform _fireTransform;           // A child of the tank where the shells are spawned.
        [SerializeField] private AudioSource _shootingAudio;         // Reference to the audio source used to play the shooting audio. NB: different to the movement audio source.
        
        public void Fire(float force)
        {
            // Create an instance of the shell and 
            // set the shell's velocity to the launch force in the fire position's forward direction.
            var shellInstance = Instantiate(_shell, _fireTransform.position, _fireTransform.rotation);
            shellInstance.velocity = force * _fireTransform.forward;

            _shootingAudio.Play();
        }
    }
}