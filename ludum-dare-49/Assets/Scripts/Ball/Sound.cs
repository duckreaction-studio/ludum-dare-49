using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ball
{
    public class Sound : MonoBehaviour
    {
        [SerializeField]
        float _velocityMaxVolume = 5f;
        [SerializeField]
        [MinMaxSlider(-5, 5, true)]
        public Vector2 _minMaxPitch = new Vector2(-1, 1);
        [SerializeField]
        public float _velocityMaxPich = 5f;

        Rigidbody _rigidbody;
        public Rigidbody rigidbody
        {
            get
            {
                if (_rigidbody == null)
                {
                    _rigidbody = GetComponent<Rigidbody>();
                }
                return _rigidbody;
            }
        }

        AudioSource _audioSource;
        public AudioSource audioSource
        {
            get
            {
                if (_audioSource == null)
                {
                    _audioSource = GetComponent<AudioSource>();
                }
                return _audioSource;
            }
        }

        public void Update()
        {
            var speed = rigidbody.velocity.magnitude;
            audioSource.volume = Mathf.Lerp(0, 1, speed / _velocityMaxVolume);
            audioSource.pitch = Mathf.Lerp(_minMaxPitch.x, _minMaxPitch.y, speed / _velocityMaxPich);
        }
    }
}