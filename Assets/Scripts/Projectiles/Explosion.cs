using UnityEngine;

namespace Projectiles
{
    public class Explosion : MonoBehaviour
    {
        private AudioSource _explosionAudio;
        private ParticleSystem _explosionParticles;

        private void Awake()
        {
            _explosionAudio = GetComponent<AudioSource>();
            _explosionParticles = GetComponent<ParticleSystem>();
            gameObject.SetActive(false);
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            transform.parent = null;
            _explosionParticles.Play();
            _explosionAudio.Play();
            Destroy(_explosionParticles.gameObject, _explosionParticles.main.duration);
        }
    }
}